using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoverShooter;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class OverlayGun : MonoBehaviour
{
    private static readonly int Active = Animator.StringToHash("Active");
    private static readonly int InActive = Animator.StringToHash("InActive");
    private static readonly int Fire = Animator.StringToHash("Fire");
    
    
    //========== Shooting Factors
    [SerializeField] private Transform gunTip;
    [SerializeField] private LayerMask shootableLayer;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float timeBetweenShots = 0.5f;
    [SerializeField] private bool BothHands;
    
    
    //======= References
    [SerializeField] private AudioClip _FireSound;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private GameObject _muzzleFlash2;
    
    private AudioSource _audioSource;
    private Animator _anim;
    private Camera _playerCamera;
    private GameObject _bullet;
    private bool _canShoot = true;
    private Hit _currentHit;

    public static Action OnGunShoot;
    public OverlayWeapons weaponType;

    private float timer = 1f;
    private AIPlayerMovement _aiPlayer;
    private Transform _nearestBone;

    private void OnEnable()
    {
        _anim.SetTrigger(Active);
        timer = 1f;
    }
    private void OnDisable()
    {
        _anim.SetTrigger(InActive);
        _muzzleFlash.SetActive(false);
        _canShoot = true;
    }

    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _audioSource = transform.parent.GetComponent<AudioSource>();
        _playerCamera = Camera.main;
        _aiPlayer = FindObjectOfType<AIPlayerMovement>();
        
        _bullet = Resources.Load("Prefabs/Bullet") as GameObject;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer >= 0)
            return;
        
        Ray ray = _playerCamera.ViewportPointToRay (new Vector3(0.5f,0.5f,0));
        RaycastHit raycastHit;
        
        Debug.DrawRay(ray.origin, ray.direction * range,Color.blue);
        if (Physics.Raycast(ray , out raycastHit, range, shootableLayer))
        {
            _currentHit = new Hit(raycastHit.point, raycastHit.normal, damage, _aiPlayer.gameObject,
                raycastHit.transform.gameObject, HitType.Pistol, 0);
            
            BodyPartHealth bodyPartHealth = raycastHit.collider.GetComponent<BodyPartHealth>();
            Explodeable explodeable = raycastHit.collider.GetComponent<Explodeable>();

            if (bodyPartHealth != null)
            {
                CharacterHealth characterHealth = bodyPartHealth.Target;
                if (characterHealth.Health <= 0)
                    return;
                
                if (_canShoot && characterHealth.Health <= damage && AIGroupsHandler.isLastEnemy) //For last enemy
                {
                    _nearestBone = GetNearestBone(characterHealth.Animator, raycastHit.point);
                    StopAllCoroutines();
                    StartCoroutine(ShootWithDelay(true));
                }
                else if (_canShoot) //For All enemies
                {
                    _nearestBone = GetNearestBone(characterHealth.Animator, raycastHit.point);
                    StopAllCoroutines();
                    StartCoroutine(ShootWithDelay(false));
                }

                OnGunShoot?.Invoke();
            }
            
            if (_canShoot && explodeable != null) //For explodeables
            {
                StopAllCoroutines();
                StartCoroutine(ShootWithDelayExplode(explodeable));
            }

            

        }
    }

    IEnumerator ShootWithDelayExplode(Explodeable e)
    {
        _canShoot = false;
        
        _audioSource.PlayOneShot(_FireSound);
        _anim.SetTrigger(Fire);
        
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.1f);
        _muzzleFlash.SetActive(false);
        
        e.Detonate();
        
        yield return new WaitForSeconds(timeBetweenShots);
        _canShoot = true;
    }
    IEnumerator ShootWithDelay(bool last)
    {
        _canShoot = false;
        
        FireProjectile(last);
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.1f);
        _muzzleFlash.SetActive(false);
        
        yield return new WaitForSeconds(timeBetweenShots);
        

        if (BothHands)
        {
            yield return new WaitForSeconds(1f);
            
            _muzzleFlash2.SetActive(true);
            yield return new WaitForSeconds(.1f);
            _muzzleFlash2.SetActive(false);
            
        }
        _canShoot = true;
            
    }
    
    Transform GetNearestBone(Animator animator,Vector3 point)
    {
        Transform nearestBone = null;
        float nearestDistance = float.MaxValue;

        foreach (HumanBodyBones boneType in System.Enum.GetValues(typeof(HumanBodyBones)))
        {
            if (boneType != HumanBodyBones.LastBone && animator.GetBoneTransform(boneType) != null)
            {
                Transform bone = animator.GetBoneTransform(boneType);
                float distance = Vector3.Distance(point, bone.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestBone = bone;
                }
            }
        }

        return nearestBone;
    }

    void FireProjectile(bool last, bool explode = false)
    {
        if (_bullet != null)
        {
            var bullet = Instantiate(_bullet);
            var gunTipPosition = gunTip.transform.position;
            
            bullet.transform.position = gunTipPosition;
            bullet.transform.LookAt(_currentHit.Position);

            var projectile = bullet.GetComponent<Projectile>();
            var vector = _currentHit.Position - gunTipPosition;
            
            if(!explode) vector = _nearestBone.position - gunTipPosition;
            
            if (last)
            {
                projectile.transform.GetChild(3).gameObject.SetActive(true);
                projectile.Speed = 15000f;
                projectile.isLast = true;
                Time.timeScale = 0.001f;
                
                CharacterStates.playerState = PlayerCustomStates.InActive;
            }

            var trail = bullet.GetComponent<TrailRenderer>();
            if (trail == null) trail = bullet.GetComponentInChildren<TrailRenderer>();

            if (trail != null)
                trail.Clear();

            if (projectile != null)
            {
                projectile.Distance = vector.magnitude;
                projectile.Direction = vector.normalized;
            }

            

            projectile.Hit = _currentHit;
            projectile.Target = _currentHit.Target;

            bullet.SetActive(true);
        }
        
        _anim.SetTrigger(Fire);
        _audioSource.PlayOneShot(_FireSound);

        
    }

    
}

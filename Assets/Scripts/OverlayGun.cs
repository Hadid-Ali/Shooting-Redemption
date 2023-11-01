using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoverShooter;
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
    
    
    //======= References
    [SerializeField] private AudioClip _FireSound;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private CharacterMotor _motor;
    
    private AudioSource _audioSource;
    private Animator _anim;
    private Camera _playerCamera;
    private GameObject _bullet;
    private bool _canShoot = true;
    private Hit _currentHit;
    
    private void OnEnable()
    {
        _anim.SetTrigger(Active);
    }
    private void OnDisable()
    {
        _anim.SetTrigger(InActive);
        _canShoot = true;
    }

    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _playerCamera = Camera.main;
        
        _bullet = Resources.Load("Prefabs/Bullet") as GameObject;
    }

    void Update()
    {
        if(!_motor.IsZooming)
            return;
        
        Ray ray = _playerCamera.ViewportPointToRay (new Vector3(0.5f,0.5f,0));
        RaycastHit raycastHit;
        
        Debug.DrawRay(ray.origin, ray.direction * range,Color.blue);
        if (Physics.Raycast(ray , out raycastHit, range, shootableLayer))
        {
            _currentHit = new Hit(raycastHit.point, raycastHit.normal, damage, _motor.gameObject,
                raycastHit.transform.gameObject, HitType.Pistol, 0);

            BodyPartHealth bodyPartHealth = raycastHit.collider.GetComponent<BodyPartHealth>();
            
            if (bodyPartHealth != null )
            {
                CharacterHealth characterHealth = bodyPartHealth.Target;
                
                if (_canShoot && characterHealth.Health is >= 10 and <= 20 && AIGroupsHandler.isLastEnemy)
                {
                    StopAllCoroutines();
                    StartCoroutine(ShootWithDelay(true));
                }
                
                if (_canShoot && characterHealth.Health > 0)
                {
                    StopAllCoroutines();
                    StartCoroutine(ShootWithDelay(false));
                }
            }

        }
    }
    IEnumerator ShootWithDelay(bool last)
    {
        _canShoot = false;
        
        FireProjectile(last);
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.1f);
        _muzzleFlash.SetActive(false);
        
        yield return new WaitForSeconds(timeBetweenShots);
        
        
        _motor.EquippedWeapon.Gun.Consume(); //Consume Bullet
        if(_motor.EquippedWeapon.Gun.LoadedBulletsLeft <= 0) //Reload if Bullets are less and equal to zero
            _motor.InputReload();
        
        _canShoot = true;
    }

    void FireProjectile(bool last)
    {
        if (_bullet != null)
        {

            
            var bullet = Instantiate(_bullet);
            var gunTipPosition = gunTip.transform.position;
            
            bullet.transform.position = gunTipPosition;
            bullet.transform.LookAt(_currentHit.Position);

            var projectile = bullet.GetComponent<Projectile>();
            var vector = _currentHit.Position - gunTipPosition;
            
            if (last)
            {
                projectile.transform.GetChild(3).gameObject.SetActive(true);
                projectile.Speed = 10;
                projectile.isLast = true;
                Time.timeScale = 0.3f;
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

    IEnumerator ShootSequence()
    {
        yield return new WaitForSeconds(1f);
    }
    
}

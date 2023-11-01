using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoverShooter;
using UnityEngine.Serialization;

public class OverlayKnife : MonoBehaviour
{
    private static readonly int Active = Animator.StringToHash("Active");
    private static readonly int InActive = Animator.StringToHash("InActive");
    private static readonly int Fire = Animator.StringToHash("Fire");
    
    
    //========== Shooting Factors
    [SerializeField] private LayerMask shootableLayer;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    
    //======= References
    [SerializeField] private AudioClip _knifeSound;
    [SerializeField] private GameObject KnifeObject;
    [SerializeField] private CharacterMotor _motor;
    [SerializeField] private ThirdPersonController _input;
    
    private AudioSource _audioSource;
    private Animator _anim;
    private Camera _playerCamera;
    private GameObject _knife;
    private Hit _currentHit;


    private bool canThrowKnife ; //Wait so it dosen't throw knife right away
    
    private void OnEnable()
    {
        _anim.SetTrigger(Active);
        KnifeObject.SetActive(true);

        canThrowKnife = false;
        StartCoroutine(ThrowKnife());
    }

    IEnumerator ThrowKnife()
    {
        yield return new WaitForSeconds(1.5f);
        canThrowKnife = true;
    }
    private void OnDisable()
    {
        _anim.SetTrigger(InActive);
    }

    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _playerCamera = Camera.main;
        
        _knife = Resources.Load("Prefabs/knife") as GameObject;
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
            
            
            if (raycastHit.collider.GetComponent<BodyPartHealth>() != null )
            {
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("KnifeThrow") && canThrowKnife)
                {
                    KnifeObject.SetActive(true);
                    _anim.SetTrigger("Fire");
                }
            }

        }
    }


    void OnAnimationComplete()
    {
        KnifeObject.SetActive(false);
        
        if (_knife != null)
        {
            var knife = Instantiate(_knife);
            knife.transform.position = KnifeObject.transform.position;
            knife.transform.rotation = KnifeObject.transform.rotation;
            knife.transform.LookAt(_currentHit.Position);

            var projectile = knife.GetComponent<Projectile>();
            var vector = _currentHit.Position - KnifeObject.transform.position;
            

            if (projectile != null)
            {
                projectile.Distance = vector.magnitude;
                projectile.Direction = vector.normalized;
            }

            projectile.Hit = _currentHit;
            projectile.Target = _currentHit.Target;

            knife.SetActive(true);
        }
        _audioSource.PlayOneShot(_knifeSound);

        _input.ZoomInput = false;
    }
}

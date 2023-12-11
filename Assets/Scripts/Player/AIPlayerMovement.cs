using System;
using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using UnityEngine;
using UnityEngine.AI;

public class AIPlayerMovement : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private CharacterMotor _motor;
    private PlayerInventory _playerInventory;

    public bool _isMoving;
    public bool _rotate;
    [SerializeField] private float lerpSpeed = 2f;
    private Transform _destination;

    private static readonly int CrouchWalk = Animator.StringToHash("CrouchWalk");



    private void Awake()
    {
        _isMoving = false;
        _destination = null;
        
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _motor = GetComponent<CharacterMotor>();
        _playerInventory = GetComponent<PlayerInventory>();
        
        GameEvents.GamePlayEvents.OnEnemyGroupKilled.Register(OnEnemyGroupKilled);

        _motor.enabled = false;
    }

    private void OnDestroy()
    {
        GameEvents.GamePlayEvents.OnEnemyGroupKilled.UnRegister(OnEnemyGroupKilled);
    }

    private void Update()
    {
        if (_isMoving)
        {
            if (Vector3.Distance(transform.position, _destination.position) < 1)
            {
                OnDestinationReached();
                _isMoving = false;
            }
        }

        if (_rotate)
        {
            float positionStep = lerpSpeed * Time.deltaTime;
            float rotationStep = lerpSpeed * Time.deltaTime;


            transform.rotation = Quaternion.Lerp(transform.rotation, _destination.rotation, rotationStep);
            bool rotationCompleted = Quaternion.Angle(transform.rotation, _destination.rotation) < 0.5f;

            if (rotationCompleted)
                _rotate = false;
            
            
        }
    }
    public void OnEnemyGroupKilled(Transform coverPosition)
    {
        PlayerInputt.CanTakeInput = false;
        SetPosition(coverPosition);
        
        CharacterStates.playerState = PlayerCustomStates.InMovement;
        CustomCameraController.CameraStateChanged(CamState.Follow);
    }
    
    public void SetPosition(Transform destination)
    {
        PlayerInputt.OnUnZoom();
        
        _rotate = false;
        _isMoving = true;
        
        _destination = destination.GetChild(0);
        _animator.SetTrigger(CrouchWalk); //Animate

        _navMeshAgent.enabled = true;
        
        StartCoroutine(DelayedMotorActivation(true));
        

    }

    public void OnDestinationReached()
    {
        CharacterStates.playerState = PlayerCustomStates.InMovement;
        _isMoving = false;
        _rotate = true;
        
        if (_playerInventory.IsCurrentWeaponRifle()) //Animate
            _animator.SetTrigger(_destination.GetComponentInParent<Cover>().isHigh ? "TallCoverRifle" : "LowCoverRifle");
        else
            _animator.SetTrigger(_destination.GetComponentInParent<Cover>().isHigh ? "TallCover" : "LowCover");
        

        _playerInventory.EquipWeapon();
        
        GameEvents.GamePlayEvents.OnPlayerReachedCover.Raise();

        StartCoroutine(DelayedMotorActivation(false));

    }

    IEnumerator DelayedMotorActivation(bool val)
    {
        yield return new WaitForSeconds(1);
        _motor.enabled = !val;
        _navMeshAgent.enabled = val;

        if(val) _navMeshAgent.SetDestination(_destination.position);
        
    }
}

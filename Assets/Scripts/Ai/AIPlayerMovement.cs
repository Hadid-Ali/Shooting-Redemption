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

    public bool _isMoving;
    public bool _rotate;
    [SerializeField] private float lerpSpeed = 2f;
    private Transform _destination;
    
    public GameEvent OnCoverReached = new();
    private static readonly int CrouchWalk = Animator.StringToHash("CrouchWalk");


    private void Awake()
    {
        _isMoving = false;
        _destination = null;
        
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _motor = GetComponent<CharacterMotor>();

        _motor.enabled = false;

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

            // Lerp the position towards the target position
            transform.position = Vector3.Lerp(transform.position, _destination.position, positionStep);

            // Lerp the rotation towards the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, _destination.rotation, rotationStep);
            
            bool positionCompleted = Vector3.Distance(transform.position, _destination.position) < 0.5f;
            bool rotationCompleted = Quaternion.Angle(transform.rotation, _destination.rotation) < 0.5f;

            if (positionCompleted && rotationCompleted)
            {
                _rotate = false;
            }
            
        }
    }
    
    public void SetPosition(Transform destination)
    {
        PlayerInputt.OnUnZoom();
        
        _rotate = false;
        _isMoving = true;
        
        _destination = destination.GetChild(0);
        _animator.SetTrigger(CrouchWalk); //Animate
        
        
        StartCoroutine(DelayedMotorActivation(true));
        

    }

    public void OnDestinationReached()
    {
        CharacterStates.playerState = PlayerCustomStates.InMovement;
        _isMoving = false;
        _rotate = true;
        
        _animator.Play("LowCover");
        _animator.Play("EquipPisol");
        
        GetComponent<CharacterInventory>().Weapons[0].RightItem.SetActive(true);
        OnCoverReached.Raise();

        StartCoroutine(DelayedMotorActivation(false));

    }

    IEnumerator DelayedMotorActivation(bool val)
    {
        yield return new WaitForSeconds(1);
        _motor.enabled = !val;
        _navMeshAgent.enabled = val;
        //rb.isKinematic = val;
        
        if(val == true)
            _navMeshAgent.SetDestination(_destination.position);
        
    }
}

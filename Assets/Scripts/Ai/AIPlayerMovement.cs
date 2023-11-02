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

    private static readonly int CustomCrouchEnd = Animator.StringToHash("CrouchEndC");

    private bool _isMoving;
    private Vector3 _destinationPoint;
    private CharacterMotor _motor;
    
    public GameEvent OnCoverReached = new();


    private void Awake()
    {
        _isMoving = false;
        _destinationPoint = Vector3.zero;

        _motor = GetComponent<CharacterMotor>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        _motor.enabled = false;
    }

    private void Update()
    {
        if (_isMoving)
        {
            if (Vector3.Distance(transform.position, _destinationPoint) < 1f)
            {
                OnDestinationReached();
                _isMoving = false;
            }
        }
    }
    
    public void SetPosition(Vector3 position)
    {
        _animator.Play("CustomCrouch");
        
        _destinationPoint = position;
        _isMoving = true;
        
        _navMeshAgent.SetDestination(position);
        
        _motor.enabled = false;
        
    }

    public void OnDestinationReached()
    {
        _motor.enabled = true;
        _isMoving = false;
        
        _animator.SetTrigger(CustomCrouchEnd);
        _navMeshAgent.enabled = false;
        
        OnCoverReached.Raise();

    }

}

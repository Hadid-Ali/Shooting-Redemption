using System;
using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using CoverShooter.AI;
using UnityEngine;

public class EnemyGroupEvents : MonoBehaviour
{
    public static GameEvent<Vector3> OnEnemyGroupKilled = new();
    public static GameEvent<bool> ShowBoss = new();
    
    private AIMovement _controller;
    private Actor m_Actor;
    
    private ThirdPersonInput _InputController;
    private ThirdPersonController _InputThirdPersonController;
    private CharacterMotor _motor;


    private int counter = 0;

    private void Awake()
    {
        _controller = GetComponent<AIMovement>();
        _InputThirdPersonController = GetComponent<ThirdPersonController>();
        _InputController = GetComponent<ThirdPersonInput>();
        _motor = GetComponent<CharacterMotor>();
        
        _controller.OnCoverReached.Register(OnCoverReached);
        OnEnemyGroupKilled.Register(OnEnemiesKilledEvent);
    }

    private void Start()
    {
        CustomCameraController.CameraStateChanged(CamState.Follow);
    }

    private void OnDestroy()
    {
        _controller.OnCoverReached.Unregister(OnCoverReached);
        OnEnemyGroupKilled.UnRegister(OnEnemiesKilledEvent);
    }

    public void OnCoverReached()
    {
        StartCoroutine(ShowBossSequence());        
        
        _InputController.TakeInputGun = true;
        _InputController.DrawWeapon(1);
        
        CustomCameraController.CameraStateChanged(CamState.Idle);
    }

    IEnumerator ShowBossSequence()
    {
        yield return new WaitForSeconds(1);
        ShowBoss.Raise(true);
        yield return new WaitForSeconds(3);
        ShowBoss.Raise(false);
    }

    public void SetPosition(Vector3 coverPosition)
    {
<<<<<<< Updated upstream
        _controller.ToRunTo(coverPosition);
        _motor.InputCrouch();
=======
        _controller.SetPosition(coverPosition);
>>>>>>> Stashed changes
    }
    public void OnEnemiesKilledEvent(Vector3 coverPosition)
    {
        _InputController.TakeInputGun = false;
        _InputThirdPersonController.ZoomInput = false;
        _InputController.UndrawWeapon();
        SetPosition(coverPosition);

        
    }
}

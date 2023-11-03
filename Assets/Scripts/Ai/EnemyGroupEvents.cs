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
    
    private AIPlayerMovement _controller;
    private Actor m_Actor;
    
    private ThirdPersonInput _InputController;
    private ThirdPersonController _InputThirdPersonController;
    private CharacterMotor _motor;


    private int counter = 0;

    private bool hasboss;

    private void Awake()
    {
        _controller = GetComponent<AIPlayerMovement>();
        _InputThirdPersonController = GetComponent<ThirdPersonController>();
        _InputController = GetComponent<ThirdPersonInput>();
        _motor = GetComponent<CharacterMotor>();
        
        _controller.OnCoverReached.Register(OnCoverReached);
        OnEnemyGroupKilled.Register(OnEnemiesKilledEvent);
        
        AIGroupsHandler.hasBossE.Register(hasBossCheck);
        AIGroupsHandler.SetPlayerStartPosition.Register(SetPlayerPosition);

    }

    private void Start()
    {
        CustomCameraController.CameraStateChanged(CamState.Follow);
    }

    private void OnDestroy()
    {
        _controller.OnCoverReached.Unregister(OnCoverReached);
        OnEnemyGroupKilled.UnRegister(OnEnemiesKilledEvent);
        AIGroupsHandler.hasBossE.UnRegister(hasBossCheck);
        AIGroupsHandler.SetPlayerStartPosition.UnRegister(SetPlayerPosition);
    }

    public void hasBossCheck(bool _hasboss)
    {
        hasboss = _hasboss;
    }

    public void SetPlayerPosition(Transform t)
    {
        transform.SetPositionAndRotation(t.position, t.rotation);
    }

    public void OnCoverReached()
    {
        CharacterStates.playerState = PlayerCustomStates.CutScene;
        
        StartCoroutine(ShowBossSequence());        
        
        _InputController.TakeInputGun = true;
        _InputController.DrawWeapon(1);
        
        CustomCameraController.CameraStateChanged(CamState.Idle);
        
    }

    IEnumerator ShowBossSequence()
    {
        
        CharacterStates.playerState = PlayerCustomStates.CutScene;
        yield return new WaitForSeconds(1);
        if (hasboss)
        {
            ShowBoss.Raise(true);
            yield return new WaitForSeconds(3);
            ShowBoss.Raise(false);
        }
        CharacterStates.playerState = PlayerCustomStates.HoldingPosition;

    }

    public void SetPosition(Vector3 coverPosition)
    {
        _controller.SetPosition(coverPosition);

        _motor.InputCrouch();
        
        CharacterStates.playerState = PlayerCustomStates.InMovement;
    }
    public void OnEnemiesKilledEvent(Vector3 coverPosition)
    {
        _InputController.TakeInputGun = false;
        _InputThirdPersonController.ZoomInput = false;
        _InputController.UndrawWeapon();
        SetPosition(coverPosition);
        
    }
}

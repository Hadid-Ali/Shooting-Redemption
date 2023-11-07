using System;
using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using CoverShooter.AI;
using UnityEngine;

public class EnemyGroupEvents : MonoBehaviour
{
    public static GameEvent<Transform> OnEnemyGroupKilled = new();
    public static GameEvent<bool> ShowBoss = new();
    
    private AIPlayerMovement _controller;
    private Actor m_Actor;
    private CharacterMotor _motor;

    private Animator _animator;


    private int counter = 0;

    private bool hasboss;


    private void NotifyGunShoot()
    {
        _motor.NotifyStartGunFire();
    }
    private void Awake()
    {
        _controller = GetComponent<AIPlayerMovement>();
        _animator = GetComponent<Animator>();
        _motor = GetComponent<CharacterMotor>();
        
        _controller.OnCoverReached.Register(OnCoverReached);
        OnEnemyGroupKilled.Register(OnEnemiesKilledEvent);
        
        AIGroupsHandler.hasBossE.Register(hasBossCheck);
        AIGroupsHandler.SetPlayerStartPosition.Register(SetPlayerPosition);
        OverlayGun.OnGunShoot += NotifyGunShoot;

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
        OverlayGun.OnGunShoot -= NotifyGunShoot;
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
        StartCoroutine(ShowBossSequence());        
        
        PlayerInputt.CanTakeInput = true;
        PlayerInputt.DrawWeapon(1);
        
        CustomCameraController.CameraStateChanged(CamState.Idle);
        
        _animator.Play("LeanCoverr");
        
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

    public void SetPosition(Transform coverPosition)
    {
        _controller.SetPosition(coverPosition);
        
        CharacterStates.playerState = PlayerCustomStates.InMovement;
    }
    public void OnEnemiesKilledEvent(Transform coverPosition)
    {
        PlayerInputt.CanTakeInput = false;
        SetPosition(coverPosition);
        
    }
}

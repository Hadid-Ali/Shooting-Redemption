using System;
using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using CoverShooter.AI;
using UnityEngine;

public class EnemyGroupEvents : MonoBehaviour
{
    private AIPlayerMovement _controller;
    private Actor m_Actor;
    private Animator _animator;
    private PlayerInventory _PlayerInventory;

    private int counter = 0;
    
    private void Awake()
    {
        _controller = GetComponent<AIPlayerMovement>();
        _animator = GetComponent<Animator>();
        _PlayerInventory = GetComponent<PlayerInventory>();

        _controller.OnCoverReached.Register(OnCoverReached);
        GameEvents.GamePlayEvents.OnPlayerSpawned.Raise();

    }
    

    private void OnDestroy()
    {
        _controller.OnCoverReached.Unregister(OnCoverReached);
        GameEvents.GamePlayEvents.OnEnemyGroupKilled.UnRegister(OnEnemiesKilledEvent);
        
    }
    public void OnCoverReached()
    {
        PlayerInputt.CanTakeInput = true;
        _PlayerInventory.DrawWeapon(Dependencies.GameDataOperations.GetSelectedWeapon());
        
        CustomCameraController.CameraStateChanged(CamState.Idle);
    }



    public void SetPosition(Transform coverPosition)
    {
        _controller.SetPosition(coverPosition);
        
        CharacterStates.playerState = PlayerCustomStates.InMovement;
        CustomCameraController.CameraStateChanged(CamState.Follow);
    }
    public void OnEnemiesKilledEvent(Transform coverPosition)
    {
        PlayerInputt.CanTakeInput = false;
        SetPosition(coverPosition);
        
    }
}

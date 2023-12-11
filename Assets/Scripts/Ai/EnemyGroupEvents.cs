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

    private void Start()
    {
        GameEvents.GamePlayEvents.OnPlayerSpawned.Raise();
    }
}

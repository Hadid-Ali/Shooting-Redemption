using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using CoverShooter;
using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    private Animator anim;
    private static readonly int Effect = Animator.StringToHash("BloodEffect");


    private Action OnhitAction;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        GameEvents.GamePlayEvents.OnPlayerSpawned.Register(Initialize);

    }

    private void Initialize(Action Onhit)
    {
        OnhitAction = Onhit;
        OnhitAction += OnHit;
    }

    private void OnDestroy()
    {
        OnhitAction -= OnHit;
    }

    public void OnHit()
    {
        anim.SetTrigger(Effect);
        print("Working");
    }
}

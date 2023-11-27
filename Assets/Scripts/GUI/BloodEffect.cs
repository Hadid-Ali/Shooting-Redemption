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


    private void Awake()
    {
        anim = GetComponent<Animator>();
        GameAdEvents.GamePlayEvents.OnPlayerHit.Register(OnHit);

    }

    private void OnDestroy()
    {
        GameAdEvents.GamePlayEvents.OnPlayerHit.Register(OnHit);
    }


    public void OnHit()
    {
        anim.SetTrigger(Effect);
    }
}

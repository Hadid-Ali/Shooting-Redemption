using System;
using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using CoverShooter.AI;
using UnityEngine;
using Hit = CoverShooter.Hit;

public class NPC : MonoBehaviour
{
    private Animator anim;
    
    private bool isDead;
    private static readonly int Scared = Animator.StringToHash("Scared");
    private static readonly int Dead = Animator.StringToHash("Dead");

    private void Awake()
    {
        anim = GetComponent<Animator>();
        OverlayGun.OnGunShoot += OnAlert;

        isDead = false;
    }

    private void OnDestroy()
    {
        OverlayGun.OnGunShoot -= OnAlert;
    }

    public void OnHit(Hit hit)
    {
        isDead = true;
        OverlayGun.OnGunShoot -= OnAlert;
        anim.Play(Dead);
    }

    public void OnAlert()
    {
        if(!isDead)
            anim.SetTrigger(Scared);
    }
}

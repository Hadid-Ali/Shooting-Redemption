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
    private CharacterHealth hp;
    
    private static readonly int Scared = Animator.StringToHash("Scared");

    private bool isDead;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        OverlayGun.OnGunShoot += OnAlert;

        isDead = false;
    }

    private void OnDestroy()
    {
      //  hp.Died -= OnDead;
        OverlayGun.OnGunShoot -= OnAlert;
    }

    public void OnHit(Hit hit)
    {
        OnDead();
    }

    public void OnDead()
    {
        anim.Play("Death");
    }

    public void OnAlert()
    {
        if(isDead)
            return;
        
        anim.SetTrigger(Scared);
    }
}

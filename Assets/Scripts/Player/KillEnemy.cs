using System;
using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using UnityEngine;

public class KillEnemy : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      if (other.GetComponent<BodyPartHealth>() && other.gameObject.layer == 12)
      {
         BodyPartHealth body = other.GetComponent<BodyPartHealth>();
         var transform1 = other.transform;
         var position = transform1.position;
         
         Hit hit = new Hit(position, position, 1000, transform.gameObject,
            body.gameObject, HitType.Pistol, 0);
         
         body.OnHit(hit);
      }
   }
}

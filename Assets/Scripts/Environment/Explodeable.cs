using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using UnityEngine;

public class Explodeable : MonoBehaviour
{
    public float explosionRadius = 5f; 
    public float explosionForce = 1000f;
    public int damage = 500;
    
    public GameObject particle; 

    private bool hasExploded = false;



    public void Detonate()
    {
        if (!hasExploded)
        {
            Explode();
            
            hasExploded = true;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }
    }

    void Explode()
    {
        var tempPart = GameObject.Instantiate(this.particle, transform.position, Quaternion.identity, null);
        tempPart.SetActive(true);
        
        var colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        var count = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, Util.Colliders);

            for (int i = 0; i < count; i++)
            {
                var collider = Util.Colliders[i];

                if(collider.GetComponent<NPC>())
                    collider.GetComponent<NPC>().OnHit();
                    
                if (!collider.isTrigger)
                {
                    var part = collider.GetComponent<BodyPartHealth>();

                    if (part == null)
                    {
                        var closest = collider.transform.position;

                        if (collider.GetType() == typeof(MeshCollider))
                            if (((MeshCollider)collider).convex)
                                closest = collider.ClosestPoint(transform.position);

                        var vector = transform.position - closest;
                        var distance = vector.magnitude;

                        if (distance < explosionRadius)
                        {
                            Vector3 normal;

                            if (distance > float.Epsilon)
                                normal = vector / distance;
                            else
                                normal = (closest - collider.transform.position).normalized;

                            Apply(collider.gameObject, closest, normal, (1 - distance / explosionRadius));
                        }
                    }
                }
            }

            PlayerInputt.PlayerExposed();
    }
    private  void Apply(GameObject target, Vector3 position, Vector3 normal, float fraction)
    {
        var damage = explosionForce * fraction;

        if (damage > float.Epsilon)
        {
            var hit = new Hit(position, normal, damage, null, target, HitType.Explosion, 0);
            target.SendMessage("OnHit", hit, SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnDrawGizmos()
    {
        // Draw explosion range Gizmo in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}



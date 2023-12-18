using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using UnityEngine;

public class Barrel : MonoBehaviour, IDestructible
{
    [SerializeField] private float explosionRadius = 5f; 
    [SerializeField] private int damage = 500;
    [SerializeField] private float hitPoints = 1;
    
    public GameObject particle; 

    void Explode()
    {
        var tempPart = GameObject.Instantiate(this.particle, transform.position, Quaternion.identity, null);
        tempPart.SetActive(true);
        
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

                            Apply(collider.gameObject, closest, normal);
                        }
                    }
                }
            }

            PlayerInputt.PlayerExposed();
    }
    private void Apply(GameObject target, Vector3 position, Vector3 normal)
    {
        if (damage > float.Epsilon)
        {
            PlayerInputt player = FindObjectOfType<PlayerInputt>();
            
            var hit = new Hit(position, normal, damage, player.gameObject, target, HitType.Explosion, 0);
            target.SendMessage("OnHit", hit, SendMessageOptions.DontRequireReceiver);
        }
    }


    public void OnHitF(float damage)
    {
        hitPoints -= damage;
        if(hitPoints <= 0)
            OnDestroyF();
    }

    public void OnDestroyF()
    {
        Explode();
        GetComponent<MeshCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}



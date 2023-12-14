using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using Unity.VisualScripting;
using UnityEngine;

public class AddComponentScript : MonoBehaviour
{
    private Rigidbody[] bodyhealths;

    [ContextMenu("ADD MeshColliders")]
    private void AddMeshColliders()
    {
        MeshRenderer[] meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();

        foreach (var VARIABLE in meshRenderers)
        {
            VARIABLE.AddComponent<MeshCollider>();
        }
    }
    
    
    
    [ContextMenu("AddBodyPartHealth")]
    private void AddComponent()
    {
        bodyhealths = transform.GetComponentsInChildren<Rigidbody>();
        

        foreach (var VARIABLE in bodyhealths)
        {
            VARIABLE.isKinematic = true;
            
            if(VARIABLE.transform.GetComponent<BodyPartHealth>() == null)
                VARIABLE.transform.AddComponent<BodyPartHealth>();
            
            if(VARIABLE != null)
             if(VARIABLE.GetComponent<CharacterJoint>() != null)
                 DestroyImmediate(VARIABLE.GetComponent<CharacterJoint>());
            
            DestroyImmediate(VARIABLE);

        }
    }

    [ContextMenu("Collider Trigger")]
    private void DisableKinematic()
    {
        Collider[] col = transform.GetComponentsInChildren<Collider>();

        
        foreach (var VARIABLE in col)
        {
            VARIABLE.isTrigger = true;
        }
    }
}

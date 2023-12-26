using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using Unity.VisualScripting;
using UnityEngine;

public class AddComponentScript : MonoBehaviour
{
    private Rigidbody[] bodyhealths;
    private BodyPartHealth[] bodyParthealths;
    private Collider[] _colliders;


    [ContextMenu("Add vest to colliders")]
    private void AddVestScript()
    {
        _colliders = transform.GetComponentsInChildren<Collider>();
        foreach (var VARIABLE in _colliders)
        {
            VARIABLE.AddComponent<VestArmour>();
        }
    }
    

    [ContextMenu("ADD MeshColliders")]
    private void AddMeshColliders()
    {
        MeshRenderer[] meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();

        foreach (var VARIABLE in meshRenderers)
        {
            VARIABLE.AddComponent<MeshCollider>();
        }
    }

    [ContextMenu("BodyPartHealthIsMain")]
    private void BodyPartHealthisMain()
    {
        bodyParthealths = transform.GetComponentsInChildren<BodyPartHealth>();
        

        foreach (var VARIABLE in bodyParthealths)
        {
            VARIABLE.isMainPlayer = true;
        }
    }

    [ContextMenu("Remove Bodypart Health")]
    private void RemoveBodypoartHealths()
    {
        bodyParthealths = transform.GetComponentsInChildren<BodyPartHealth>();
        foreach (var VARIABLE in bodyParthealths)
        {
            DestroyImmediate(VARIABLE);
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

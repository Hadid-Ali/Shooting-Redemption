using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Mono.CompilerServices.SymbolWriter;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerFollowOffset : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;


    private Transform rotationTransform;
    private void Awake()
    {
        if (playerTransform == null)
            playerTransform = FindObjectOfType<AIPlayerMovement>().transform;
        
        EnemyGroupEvents.OnEnemyGroupKilled.Register(UpdateRotation);
    }



    private void OnDestroy()
    {
        EnemyGroupEvents.OnEnemyGroupKilled.UnRegister(UpdateRotation);
    }
    
    public void UpdateRotation(Transform rotation)
    {
        rotationTransform = rotation;
        
        var eulerAngles = rotation.localEulerAngles;
        
        transform.eulerAngles = new Vector3(0, eulerAngles.y, 0);
    }
    

    void LateUpdate()
    {
        var position = playerTransform.position;

        transform.position = position + Vector3.up;
    }
}

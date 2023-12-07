using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerFollowOffset : MonoBehaviour
{
    private Transform playerTransform;

    private void Awake()
    {
        GameEvents.GamePlayEvents.OnPlayerSpawned.Register(GetPlayerTransform);
        GameEvents.GamePlayEvents.OnEnemyGroupKilled.Register(UpdateFollowRotation);
    }
    

    private void GetPlayerTransform()
    {
        playerTransform = FindObjectOfType<AIPlayerMovement>().transform;
    }

    private void OnDestroy()
    {
        GameEvents.GamePlayEvents.OnEnemyGroupKilled.UnRegister(UpdateFollowRotation);
        GameEvents.GamePlayEvents.OnPlayerSpawned.Unregister(GetPlayerTransform);
    }

    private void UpdateFollowRotation(Transform obj)
    {
        transform.localEulerAngles = obj.localEulerAngles;
    }
    
    void Update()
    {
        transform.position = playerTransform.position + Vector3.up;
    }
}

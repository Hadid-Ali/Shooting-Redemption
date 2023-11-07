using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerFollowOffset : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private void Awake()
    {
        gameObject.transform.parent = null;
        EnemyGroupEvents.OnEnemyGroupKilled.Register(UpdateFollowRotation);
    }

    private void OnDestroy()
    {
        EnemyGroupEvents.OnEnemyGroupKilled.UnRegister(UpdateFollowRotation);
    }

    private void UpdateFollowRotation(Transform obj)
    {
        transform.localEulerAngles = obj.localEulerAngles;

        print("Rotation is working");
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position + Vector3.up;
    }
}

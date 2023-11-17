using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerFollowOffset : MonoBehaviour
{
    private Transform playerTransform;

    [SerializeField] private CinemachineVirtualCamera follow;
    [SerializeField] private CinemachineVirtualCamera idle;
    [SerializeField] private CinemachineVirtualCamera zoom;
    
    public static CinemachineVirtualCamera followCam;
    public static CinemachineVirtualCamera idleCam;
    public static CinemachineVirtualCamera zoomCam;

    private void Awake()
    {
        playerTransform = FindObjectOfType<AIPlayerMovement>().transform;
        EnemyGroupEvents.OnEnemyGroupKilled.Register(UpdateFollowRotation);

        followCam = follow;
        idleCam = idle;
        zoomCam = zoom;
    }

    private void OnDestroy()
    {
        EnemyGroupEvents.OnEnemyGroupKilled.UnRegister(UpdateFollowRotation);
    }

    private void UpdateFollowRotation(Transform obj)
    {
        transform.localEulerAngles = obj.localEulerAngles;
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position + Vector3.up;
    }
}

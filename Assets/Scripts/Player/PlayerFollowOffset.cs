using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

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
        followCam = follow;
        idleCam = idle;
        zoomCam = zoom;
    }

    private void Start()
    {
        playerTransform = FindObjectOfType<AIPlayerMovement>().transform;
        GameEvents.GamePlayEvents.OnEnemyGroupKilled.Register(UpdateFollowRotation);

        followCam = follow;
        idleCam = idle;
        zoomCam = zoom;
    }

    private void OnDestroy()
    {
        GameEvents.GamePlayEvents.OnEnemyGroupKilled.UnRegister(UpdateFollowRotation);
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

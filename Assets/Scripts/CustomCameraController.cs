using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using CoverShooter;
using UnityEngine;

public enum  CamState{

    Idle,
    Follow,
    Zoom
}
public class CustomCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera followCam;
    [SerializeField] private CinemachineVirtualCamera idleCam;
    [SerializeField] private CinemachineVirtualCamera zoomCam;
    
    private static readonly int CamNumber = Animator.StringToHash("CamNumber");
    public Animator _animator;

    public static Action<CamState> CameraStateChanged;
    private void Awake()
    {
        _animator.GetComponent<Animator>();
        CameraStateChanged += SetCamState;
        
        SetCamState(CamState.Follow);
        
        OverlayGunHandler.OnCustomZoom += OnZoom;
        OverlayGunHandler.OnCustomUnZoom += OnUnZoom;
    }

    private void OnDestroy()
    {
        CameraStateChanged -= SetCamState;
        
        OverlayGunHandler.OnCustomZoom -= OnZoom;
        OverlayGunHandler.OnCustomUnZoom -= OnUnZoom;
    }

    private void OnZoom()
    {
        if(CharacterStates.playerState != PlayerCustomStates.HoldingPosition)
            return;
        
        if(!AIGroupsHandler.isLastEnemy)
            SetCamState(CamState.Zoom);
    }

    private void OnUnZoom()
    {
        if(!AIGroupsHandler.isLastEnemy)
            SetCamState(CamState.Idle);
    }

    private void SetCamState(CamState state)
    {
        switch (state)
        {
            case CamState.Follow:
                followCam.Priority = 10;
                idleCam.Priority = 5;
                zoomCam.Priority = 6;
                break;
            case CamState.Idle:
                followCam.Priority = 5;
                idleCam.Priority = 10;
                zoomCam.Priority = 6;
                break;
            case CamState.Zoom:
                followCam.Priority = 6;
                idleCam.Priority = 5;
                zoomCam.Priority = 10;
                break;
        }
    }


}

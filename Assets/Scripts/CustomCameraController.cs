using System;
using Cinemachine;
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
    

    public static Action<CamState> CameraStateChanged;
    private void Awake()
    {
        CameraStateChanged += SetCamState;
        
        SetCamState(CamState.Follow);
        
        PlayerInputt.OnZoom += OnZoom;
        PlayerInputt.OnUnZoom += OnUnZoom;
    }

    private void OnDestroy()
    {
        CameraStateChanged -= SetCamState;
        
        PlayerInputt.OnZoom -= OnZoom;
        PlayerInputt.OnUnZoom -= OnUnZoom;
    }

    private void OnZoom()
    {
        if(CharacterStates.playerState != PlayerCustomStates.HoldingPosition)
            return;
        
        SetCamState(CamState.Zoom);
    }

    private void OnUnZoom()
    {
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

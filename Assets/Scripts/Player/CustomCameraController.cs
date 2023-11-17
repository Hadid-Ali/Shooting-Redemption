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
    private CinemachineVirtualCamera followCam;
    private CinemachineVirtualCamera idleCam;
    private CinemachineVirtualCamera zoomCam;
    

    public static Action<CamState> CameraStateChanged;

    private void Start()
    {
        followCam = PlayerFollowOffset.followCam;
        idleCam = PlayerFollowOffset.idleCam;
        zoomCam = PlayerFollowOffset.zoomCam;
        
        CameraStateChanged += SetCamState;
        PlayerInputt.OnZoom += OnZoom;
        PlayerInputt.OnUnZoom += OnUnZoom;
        
        SetCamState(CamState.Follow);
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

using System;
using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using UnityEngine;

public enum  CamState{

    Idle,
    Follow,
    Zoom
}
public class CustomCameraController : MonoBehaviour
{
    private static readonly int CamNumber = Animator.StringToHash("CamNumber");
    public Animator _animator;

    public static Action<CamState> CameraStateChanged;
    private void Awake()
    {
        _animator.GetComponent<Animator>();
        CameraStateChanged += SetCamState;
        
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
                _animator.SetInteger(CamNumber,0);
                break;
            case CamState.Idle:
                _animator.SetInteger(CamNumber,1);
                break;
            case CamState.Zoom:
                _animator.SetInteger(CamNumber,2);
                break;
        }
    }


}

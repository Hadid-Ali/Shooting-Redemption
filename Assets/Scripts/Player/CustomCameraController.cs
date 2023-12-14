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
    [SerializeField] private CinemachineVirtualCamera playerCam;

    private CinemachineCameraOffset _offset;
    
    public float duration = 2.0f; 
    float _elapsedTime = 0f;

    [SerializeField] private Vector3 idleOffset = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 zoomOffset = new Vector3(0, 0, 2);
    
    private Vector3 startOffset = new Vector3(0, 0, 0);
    private Vector3 endOffset = new Vector3(0, 0, 0);
    
    
    private CamState _currentState;
    public static Action<CamState> CameraStateChanged;

    private void Awake()
    {
        CameraStateChanged += SetCamState;
        PlayerInputt.OnZoom += OnZoom;
        PlayerInputt.OnUnZoom += OnUnZoom;
        _offset = playerCam.GetComponent<CinemachineCameraOffset>();
        
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

    private void Update()
    {
        if (_elapsedTime < duration)
        {
            _elapsedTime += Time.deltaTime; // Use Time.deltaTime for frame-independent time

            float t = _elapsedTime / duration;
            Vector3 currentOffset = Vector3.Lerp(startOffset, endOffset, t);
            
            _offset.m_Offset = currentOffset;
        }
        else
        {
            _offset.m_Offset = endOffset;
        }
        
    }

    private void SetCamState(CamState state)
    {
        switch (state)
        {
            case CamState.Follow:
                followCam.Priority = 10;
                playerCam.Priority = 5;
                _currentState = CamState.Follow;
                break;
            case CamState.Idle:
                followCam.Priority = 5;
                playerCam.Priority = 10;
                startOffset = zoomOffset;
                endOffset = idleOffset;
                
                _currentState = CamState.Idle;
                _elapsedTime = 0;
                break;
            case CamState.Zoom:
                followCam.Priority = 5;
                playerCam.Priority = 10;
                
                startOffset = idleOffset;
                endOffset = zoomOffset;
                
                _elapsedTime = 0;
                _currentState = CamState.Zoom;
                break; 
        }
    }


}

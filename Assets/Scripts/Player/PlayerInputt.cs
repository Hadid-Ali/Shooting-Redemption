using System;
using System.Security.Cryptography;
using CoverShooter;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlayerInputt : MonoBehaviour
{
    public static Action PlayerExposed;
    
    public static Action OnZoom;
    public static Action OnUnZoom;
    public static Action OnGunChangeInput;

    public static bool CanTakeInput;

    private void Awake()
    {
        PlayerExposed += PlayerExposedF;
    }

    private void OnDestroy()
    {
        PlayerExposed -= PlayerExposedF;
    }

    void Update()
    {
        if(!CanTakeInput)
            return;
        
        if (ControlFreak2.CF2Input.GetButtonDown("Zoom"))
        {
            if(CharacterStates.playerState == PlayerCustomStates.HoldingPosition) 
                OnZoom();
            
            CharacterStates.SetPlayerState(PlayerCustomStates.InZoom);
        }
        
        if (ControlFreak2.CF2Input.GetButtonUp("Zoom"))
        {
            PlayerExposed();
            OnUnZoom();
            
            CharacterStates.SetPlayerState(PlayerCustomStates.HoldingPosition);
        }

        if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Alpha0))
        {
            OnGunChangeInput();
        }
    }

    public void PlayerExposedF()
    {
        GetComponent<Actor>().enabled = true;
    }

}

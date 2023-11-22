using System;
using CoverShooter;
using UnityEngine;

public class PlayerInputt : MonoBehaviour
{
    
    public static Action OnZoom;
    public static Action OnUnZoom;
    public static Action OnGunChangeInput;

    public static bool CanTakeInput;
    
    void Update()
    {
        if(!CanTakeInput)
            return;
        
        if (ControlFreak2.CF2Input.GetButtonDown("Zoom"))
        {
            if(CharacterStates.playerState == PlayerCustomStates.HoldingPosition) 
                OnZoom();
            
            CharacterStates.playerState = PlayerCustomStates.InZoom;
        }
        
        if (ControlFreak2.CF2Input.GetButtonUp("Zoom"))
        {
            GetComponent<Actor>().enabled = true;
            
            OnUnZoom();
            
            CharacterStates.playerState = PlayerCustomStates.HoldingPosition;
        }

        if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Alpha0))
        {
            OnGunChangeInput();
        }
    }

}

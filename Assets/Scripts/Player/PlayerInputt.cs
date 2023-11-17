using System;
using CoverShooter;
using UnityEngine;

public class PlayerInputt : MonoBehaviour
{
    
    public static Action OnZoom;
    public static Action OnUnZoom;
    public static Action<int> OnGunChange;

    public static bool CanTakeInput;

    private int gunCounter;


    private void Awake()
    {
        gunCounter = 0;
    }

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
            gunCounter++;
            if (gunCounter > PlayerInventory.guns + 1)
                gunCounter = 0;

            OnGunChange(gunCounter);
        }


    }

    public static void DrawWeapon(int i)
    {
        OnGunChange(i);
    }
}

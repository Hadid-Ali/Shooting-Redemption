using System;
using UnityEngine;

public class PlayerInputt : MonoBehaviour
{
    
    public static Action OnZoom;
    public static Action OnUnZoom;
    public static Action<int> OnGunChange;

    public static bool CanTakeInput;

    // Update is called once per frame
    void Update()
    {
        if(!CanTakeInput)
            return;
        
        if (ControlFreak2.CF2Input.GetButtonDown("Zoom"))
        {
            OnZoom();
            CharacterStates.playerState = PlayerCustomStates.InZoom;
        }
        
        if (ControlFreak2.CF2Input.GetButtonUp("Zoom"))
        {
            OnUnZoom();
            CharacterStates.playerState = PlayerCustomStates.HoldingPosition;
        }

        if (ControlFreak2.CF2Input.GetKey(KeyCode.Alpha1)){OnGunChange(0);}
        if (ControlFreak2.CF2Input.GetKey(KeyCode.Alpha2)){OnGunChange(1);}
        if (ControlFreak2.CF2Input.GetKey(KeyCode.Alpha3)){OnGunChange(2);}
        if (ControlFreak2.CF2Input.GetKey(KeyCode.Alpha4)){OnGunChange(3);}

    }

    public static void DrawWeapon(int i)
    {
        OnGunChange(i);
    }
}

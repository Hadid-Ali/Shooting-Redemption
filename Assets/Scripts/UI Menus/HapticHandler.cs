using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HapticHandler 
{
    public static void Vibrate()
    {
        if (SaveLoadData.GameData.haptic)
        {
            Handheld.Vibrate();
            Debug.Log("vibratrion");
        }
    }
}

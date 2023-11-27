using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HapticHandler 
{
    public static void Vibrate()
    {
        if (Dependencies.GameDataOperations.GetHapticSound())
        {
            Handheld.Vibrate();
            Debug.Log("vibratrion");
        }
    }
}

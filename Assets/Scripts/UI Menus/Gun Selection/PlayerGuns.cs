using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerGuns 
{
    public GunName Guns;
    public bool isUnlocked;
    public bool isSelected;

    public void SetGunUnlocked()
    {
        isUnlocked = true;
    }
    public bool GetUnlocked() => isUnlocked;
    public bool GetSelected() => isSelected;

    public void DeselectGun()
    {
        isSelected = false;
    }
    
    public void SelectGun()
    {
        isSelected = true;
    }
}

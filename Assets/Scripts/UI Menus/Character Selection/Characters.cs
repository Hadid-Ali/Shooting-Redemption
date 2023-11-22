using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class ItemSelection
{
    public bool isUnlocked;
    public bool isSelected;

    public void SetGunUnlocked()
    {
        isUnlocked = true;
    }
    public bool GetUnlocked() => isUnlocked;
    public bool GetSelected() => isSelected;

    public void DeselectItem()
    {
        isSelected = false;
    }
    
    public void SelectItem()
    {
        isSelected = true;
    }
}
[System.Serializable]
public class Characters : ItemSelection
{
    public CharacterType Character;
}
[System.Serializable]
public class Guns : ItemSelection
{
    public GunName Gun;
}

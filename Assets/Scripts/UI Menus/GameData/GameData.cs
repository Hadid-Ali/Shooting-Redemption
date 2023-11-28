using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

[CreateAssetMenu(fileName = "GameData", menuName = "GameData/GameData/Create", order = 1)]
public class GameData : ScriptableObject
{
    public bool sound;

    public int m_Credits;
    public int m_SelectedLevel;
    public int m_SelectedEpisode;
    
    public int[] m_UnlockedLevels = new int[5];
    public int m_UnlockedEpisodes;
    
    public Dictionary<CharacterType, bool> Characters = new();
    public Dictionary<OverlayWeapons, bool> Guns = new();

    public OverlayWeapons selectedGun;
    public CharacterType selectedCharacter;

    public OverlayWeapons[] CurrentWeapons;
}

public class ItemStatus
{
    public ItemStatus(bool _isUnlocked, bool _isSelected)
    {
        isUnlocked = _isUnlocked;
        isSelected = _isSelected;

    }
    public bool isUnlocked = false;
    public bool isSelected = false;
}

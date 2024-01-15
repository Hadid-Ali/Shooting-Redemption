using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "GameData/GameData/Create", order = 1)]
public class GameData : ScriptableObject
{
    public bool hasconsent;
    public bool hasShownTutorial;
    public bool sound;
    
    public int m_Credits;
    public int m_SelectedLevel;
    public int m_SelectedEpisode;
    
    public int[] m_UnlockedLevels = new int[5];
    public bool[] m_UnlockedEpisodes = new bool[5];
    
    public List<CharacterStatus> Characters = new();
    public List<GunStatus> Guns = new();

    public OverlayWeapons selectedGun;
    public CharacterType selectedCharacter;
}

[Serializable]
public class GunStatus
{
    public OverlayWeapons weapon;
    public bool isUnlocked = false;
}
[Serializable]
public class CharacterStatus
{
    public CharacterType character;
    public bool isUnlocked = false;
}

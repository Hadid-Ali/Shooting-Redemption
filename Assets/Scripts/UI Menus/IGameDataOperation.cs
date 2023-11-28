using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameDataOperation
{
    public void LoadData();
    public void SaveData();


    public bool GetSoundStatus();
    public void SetSoundStatus(bool val);
    public void SetCredit(int i);
    public int GetCredits();

    //Levels
    public void SetUnlockedLevels(int episode, int levels);
    public int GetUnlockedLevels(int episode);
    public void SetUnlockedEpisodes(int episodes);
    public int GetUnlockedEpisodes();
    public void SetSelectedLevel(int level);
    public int GetSelectedLevel();
    public int GetSelectedEpisode();
    public void SetSelectedEpisode(int episode);
    
    //Character and Guns
    public Dictionary<OverlayWeapons, bool> GetAllWeapons();
    public void SetCharacterData(CharacterType character, bool unlocked);
    public void SetGunData(OverlayWeapons weapon, bool unlocked);
    public bool GetCharacterUnlocked(CharacterType character);
    public bool GetGunUnlocked(OverlayWeapons weapon);
    public OverlayWeapons GetSelectedWeapon();
    public void SetSelectedWeapon(OverlayWeapons weapon);

    public CharacterType GetSelectedCharacterType();
    public void SetSelectedCharacterType(CharacterType character);
}
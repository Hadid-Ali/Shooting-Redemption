using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameDataOperation
{
    public void LoadData();
    public void SaveData();

    public bool GetSoundStatus();
    public void SetSoundStatus(bool val);
    public void AddCredits(int i);
    public void SubtractCredits(int i);
    public int GetCredits();

    public void SetTutorialShown(bool val);
    public bool GetTutorialShown();

    //Levels
    public void SetUnlockedLevels(int episode, int levels);
    public int GetUnlockedLevels(int episode);
    public void SetUnlockedEpisodes(int episodes);
    public bool GetUnlockedEpisodes(int episode);
    public int GetSelectedLevel();
    public int GetSelectedEpisode();
    public void SetSelectedEpisode(int episode);
    public void SetSelectedLevel(int level);
    
    //Character and Guns
    public List<GunStatus> GetAllWeaponsData();
    public List<CharacterStatus> GetAllCharactersData();
    public void SetCharacterData(CharacterStatus status);
    public void SetGunData(GunStatus status);
    public CharacterStatus GetCharacterData(CharacterType character);
    public GunStatus GetGunData(OverlayWeapons weapon);

    public CharacterType GetSelectedCharacter();
    public OverlayWeapons GetSelectedWeapon();
    public void SetSelectedCharacter(CharacterType characterType);
    public void SetSelectedWeapon(OverlayWeapons weapon);

}
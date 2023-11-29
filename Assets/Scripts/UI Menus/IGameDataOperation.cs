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
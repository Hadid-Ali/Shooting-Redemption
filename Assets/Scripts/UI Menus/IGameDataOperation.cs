using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameDataOperation
{
    public void LoadData();
    public void SaveData();

    public bool GetCharacterUnlocked(CharacterType characterType);
    public bool GetSelectedCharacter(CharacterType characterType);
    public void SetSelectedCharacter(CharacterType characterType);
    public void SetCharacterUnlocked(CharacterType characterTypee);

    public int SetSelectedCharacterIndex(CharacterType characterTypee);
    public int GetSelectedCharacterIndex();


    public bool GetGunUnlocked(GunName gunName);
    public bool GetSelectedGun(GunName gunName);
    public void SetSelectedGun(GunName gunName);
    public void SetGunUnlocked(GunName gunName);
   // public int SetSelectedGunIndex(GunName gunName);
    public int GetSelectedGunIndex();

    public void DeselectAllCharacters();
    public void DeselectAllGuns();
    public int GetSelectedEpisode();
    
    public int GetSelectedLevel(int i);
    public void SetSelectedlevel(int i);
    public void SetSelectedEpisode(int i);
    
    //Unlocking Setter Getters
    public void SetUnlockedLevels(int index, int val);
    public void SetUnlockedEpisodes(int val);

    public int GetUnlockedLevels(int index);
    public int GetUnlockedEpisodes();


    public void SetHapticSound(bool i);
    public bool GetHapticSound();
    public void SetSound(bool i);
    public bool GetSound();
    public void SetShadow(bool i);
    public bool GetShadow();

    public int SetCoins();
    public void GetCoins(int i);

    public int SetSelectedGunIndex(int i);

}
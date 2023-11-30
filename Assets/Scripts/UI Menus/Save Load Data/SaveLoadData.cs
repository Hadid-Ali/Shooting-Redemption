using System;
using System.Collections.Generic;
using System.Linq;
using ControlFreak2.Demos.Guns;
using UnityEngine;

public class SaveLoadData : MonoBehaviour, IGameDataOperation
{
    [SerializeField] private GameData m_GameData;

    private void Awake()
    {
        LoadData();
    }

    private void Start()
    {
        Dependencies.GameDataOperations = this;
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey("Save"))
        {
            string saveJson = PlayerPrefs.GetString("Save");
            JsonUtility.FromJsonOverwrite(saveJson, m_GameData);
           // OnGameDataLoaded();
        }
        else
        {
           // InitData();\
           print("No Data Found");
        }
    }

    private void OnGameDataLoaded()
    {
        // for (int i = 0; i < m_GameData.CurrentWeapons.Length; i++)
        // {
        //     if (m_GameData.Guns.Keys.Contains(m_GameData.CurrentWeapons[i]))
        //         continue;
        //
        //     m_GameData.Guns[m_GameData.CurrentWeapons[i]] = false;
        // }
        // print("working");
    }
    
    private void InitData()
    {
        // for (int i = 0; i < m_GameData.CurrentWeapons.Length; i++)
        // {
        //     OverlayWeapons weapon = m_GameData.CurrentWeapons[i];
        //     m_GameData.Guns[weapon] = weapon == m_GameData.selectedGun;
        // }
        // print("working");
    }

    void IGameDataOperation.SaveData()
    {
        string saveJson = JsonUtility.ToJson(m_GameData);
        PlayerPrefs.SetString("Save", saveJson);
        PlayerPrefs.Save();
    }


    public bool GetSoundStatus() => m_GameData.sound;
    public void SetSoundStatus(bool val) => m_GameData.sound = val;

    //Credits
    public void SetCredit(int i) => m_GameData.m_Credits = i;
    public int GetCredits() => m_GameData.m_Credits;

    //Levels and Episodes
    public void SetUnlockedLevels(int episode, int levels) => m_GameData.m_UnlockedLevels[episode] = levels;
    public int GetUnlockedLevels(int episode) => m_GameData.m_UnlockedLevels[episode];
    public void SetUnlockedEpisodes(int episode) => m_GameData.m_UnlockedEpisodes[episode] = true;
    public bool GetUnlockedEpisodes(int episode) => m_GameData.m_UnlockedEpisodes[episode];
    public void SetSelectedLevel(int level) => m_GameData.m_SelectedLevel = level;
    public int GetSelectedLevel() => m_GameData.m_SelectedLevel;
    public int GetSelectedEpisode() => m_GameData.m_SelectedEpisode;
    public void SetSelectedEpisode(int episode) => m_GameData.m_SelectedEpisode = episode;


    //Character and Guns
    public List<GunStatus> GetAllWeaponsData() => m_GameData.Guns;
    public List<CharacterStatus> GetAllCharactersData() => m_GameData.Characters;

    public void SetCharacterData(CharacterStatus characterStatus)
    {
        CharacterStatus status = m_GameData.Characters.Find(x => x.character == characterStatus.character);
        status = characterStatus;
    }

    public void SetGunData(GunStatus gunStatus)
    {
        GunStatus status =  m_GameData.Guns.Find(x => x.weapon == gunStatus.weapon);
        status = gunStatus;
    }
    public CharacterStatus GetCharacterData(CharacterType character) => m_GameData.Characters.Find(x=> x.character == character);
    public GunStatus GetGunData(OverlayWeapons weapon) => m_GameData.Guns.Find(x=> x.weapon == weapon);
    public CharacterType GetSelectedCharacter() => m_GameData.selectedCharacter;
    public OverlayWeapons GetSelectedWeapon() => m_GameData.selectedGun;
    public void SetSelectedCharacter(CharacterType characterType) => m_GameData.selectedCharacter = characterType;
    public void SetSelectedWeapon(OverlayWeapons weapon) => m_GameData.selectedGun = weapon;
}

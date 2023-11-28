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
            OnGameDataLoaded();
        }
        else
        {
            InitData();
        }
    }

    private void OnGameDataLoaded()
    {
        for (int i = 0; i < m_GameData.CurrentWeapons.Length; i++)
        {
            if (m_GameData.Guns.Keys.Contains(m_GameData.CurrentWeapons[i]))
                continue;

            m_GameData.Guns[m_GameData.CurrentWeapons[i]] = false;
        }
    }
    
    private void InitData()
    {
        for (int i = 0; i < m_GameData.CurrentWeapons.Length; i++)
        {
            OverlayWeapons weapon = m_GameData.CurrentWeapons[i];
            m_GameData.Guns[weapon] = weapon == m_GameData.selectedGun;
        }
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
    public void SetUnlockedEpisodes(int episodes) => m_GameData.m_UnlockedEpisodes = episodes;
    public int GetUnlockedEpisodes() => m_GameData.m_UnlockedEpisodes;
    public void SetSelectedLevel(int level) => m_GameData.m_SelectedLevel = level;
    public int GetSelectedLevel() => m_GameData.m_SelectedLevel;
    public int GetSelectedEpisode() => m_GameData.m_SelectedEpisode;
    public void SetSelectedEpisode(int episode) => m_GameData.m_SelectedEpisode = episode;
    public Dictionary<OverlayWeapons, bool> GetAllWeapons() => m_GameData.Guns;


    //Character and Guns
    public void SetCharacterData(CharacterType character, bool unlocked) => m_GameData.Characters[character] = unlocked;
    public void SetGunData(OverlayWeapons weapon, bool unlocked) => m_GameData.Guns[weapon] = unlocked;
    public bool GetCharacterUnlocked(CharacterType character) => m_GameData.Characters[character];
    public bool GetGunUnlocked(OverlayWeapons weapon) => m_GameData.Guns[weapon];
    public OverlayWeapons GetSelectedWeapon() => m_GameData.selectedGun;
    public void SetSelectedWeapon(OverlayWeapons weapon) => m_GameData.selectedGun = weapon;
    public CharacterType GetSelectedCharacterType() => m_GameData.selectedCharacter;
    public void SetSelectedCharacterType(CharacterType character) => m_GameData.selectedCharacter = character;
}

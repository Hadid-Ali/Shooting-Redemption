using System;
using UnityEngine;
public class SaveLoadData : MonoBehaviour,IGameDataOperation
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
           // m_GameData = GameData;
        }
        else
        {
            m_GameData.DateTime = DateTime.Now.ToBinary().ToString();
        }
    }

    void IGameDataOperation.SaveData()
    {
        string saveJson = JsonUtility.ToJson(m_GameData);
        PlayerPrefs.SetString("Save",saveJson);
        PlayerPrefs.Save();
    }

   public bool GetCharacterUnlocked(CharacterType characterType) => m_GameData.CharacterData.Find(x => x.Character == characterType).GetUnlocked();
   public bool GetSelectedCharacter(CharacterType characterType) => m_GameData.CharacterData.Find(x => x.Character == characterType).GetSelected();
   public void SetSelectedCharacter(CharacterType characterType) => m_GameData.CharacterData.Find(x => x.Character == characterType).SelectItem();
   public void SetCharacterUnlocked(CharacterType characterTypee) => m_GameData.CharacterData.Find(x => x.Character == characterTypee).SetGunUnlocked();
   
   
   public bool GetGunUnlocked(GunName gunName) => m_GameData.GunsData.Find(x => x.Gun == gunName).GetUnlocked();
   public bool GetSelectedGun(GunName gunName) => m_GameData.GunsData.Find(x => x.Gun == gunName).GetSelected();
   public void SetSelectedGun(GunName gunName) => m_GameData.GunsData.Find(x => x.Gun == gunName).SelectItem();
   public void SetGunUnlocked(GunName gunName) => m_GameData.GunsData.Find(x => x.Gun == gunName).SetGunUnlocked();
   
   
   //Level/Episode Selection
   public int GetSelectedEpisode() => m_GameData.m_SelectedEpisode;
   public int GetSelectedLevel(int i) => m_GameData.m_SelectedLevel;
   public void SetSelectedlevel(int i) => m_GameData.m_SelectedLevel = i;
   public void SetSelectedEpisode(int i)
   {
       print("ftgyhjftyujk");
       m_GameData.m_SelectedEpisode = i;
   }


   //Level/Episode Unlocking
   public void SetUnlockedLevels(int index, int val) => m_GameData.m_UnlockedLevels[index] = val;
   public void SetUnlockedEpisodes(int index) => m_GameData.m_UnlockedEpisodes = index;

   public int GetUnlockedLevels(int index) => m_GameData.m_UnlockedLevels[index];
   public int GetUnlockedEpisodes() => m_GameData.m_UnlockedEpisodes;
   
   

   public void SetHapticSound(bool i) => m_GameData.haptic = i;
   public bool GetHapticSound() => m_GameData.haptic;
   public void SetSound(bool i) => m_GameData.sound = i;


   public bool GetSound() => m_GameData.sound;

   public void SetShadow(bool i) => m_GameData.shadow = i;

   public bool GetShadow() => m_GameData.shadow;

   public void SetCoins(int i) => m_GameData.m_Coins = i;
   public int GetCoins() => m_GameData.m_Coins;
   public int SetSelectedGunIndex(int i) => m_GameData.m_SelectedGunIndex = i;
   

   public int SetSelectedCharacterIndex(CharacterType characterTypee) => m_GameData.m_SelectedCharacterIndex  = (int)characterTypee;
   public int GetSelectedCharacterIndex() => m_GameData.m_SelectedCharacterIndex;
   
  // public int SetSelectedGunIndex(GunName gunName) => m_GameData.m_SelectedCharacterIndex  = (int)gunName;
   public int GetSelectedGunIndex() => m_GameData.m_SelectedCharacterIndex;

   public bool SetSound() => m_GameData.sound;
   public bool SetShadow() => m_GameData.shadow;

   /*public bool IsLevelCompleted(int episode, int level) => m_GameData.IsLevelCompleted(episode, level);*/

   public void DeselectAllCharacters()
   {
       for (int i = 0; i < m_GameData.CharacterData.Count; i++)
       {
           m_GameData.CharacterData[i].DeselectItem();
       }
   }
   
   public void DeselectAllGuns()
   {
       for (int i = 0; i < m_GameData.GunsData.Count; i++)
       {
           m_GameData.GunsData[i].DeselectItem();
       }
   }
}

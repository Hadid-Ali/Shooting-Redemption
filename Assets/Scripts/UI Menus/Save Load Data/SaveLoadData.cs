using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveLoadData : MonobehaviourSingleton<SaveLoadData>
{
    [SerializeField]  GameData m_GameData;

    public static GameData GameData;


    private void Awake()
    {
        GameData = m_GameData;
        loadData();
        
     }

   public void loadData()
    {
        if (PlayerPrefs.HasKey("Save"))
        {
            string saveJson = PlayerPrefs.GetString("Save");
            JsonUtility.FromJsonOverwrite(saveJson, GameData);
            m_GameData = GameData;
        }
        else
        {
            GameData.DateTime = DateTime.Now.ToBinary().ToString();
        }
    }

   public static void SaveData()
    {
        string saveJson = JsonUtility.ToJson(GameData);
        PlayerPrefs.SetString("Save",saveJson);
        PlayerPrefs.Save();
    }

   
   public bool GetCharacterUnlocked(CharacterType characterType) => GameData.CharacterData.Find(x => x.Character == characterType).GetUnlocked();
   public bool GetSelectedCharacter(CharacterType characterType) => GameData.CharacterData.Find(x => x.Character == characterType).GetSelected();
   public void SetSelectedCharacter(CharacterType characterType) => GameData.CharacterData.Find(x => x.Character == characterType).SelectItem();
   public void SetCharacterUnlocked(CharacterType characterTypee) => GameData.CharacterData.Find(x => x.Character == characterTypee).SetGunUnlocked();
   
   
   public bool GetGunUnlocked(GunName gunName) => GameData.GunsData.Find(x => x.Gun == gunName).GetUnlocked();
   public bool GetSelectedGun(GunName gunName) => GameData.GunsData.Find(x => x.Gun == gunName).GetSelected();
   public void SetSelectedGun(GunName gunName) => GameData.GunsData.Find(x => x.Gun == gunName).SelectItem();
   public void SetGunUnlocked(GunName gunName) => GameData.GunsData.Find(x => x.Gun == gunName).SetGunUnlocked();

   
   

   public void DeselectAllCharacters()
   {
       for (int i = 0; i < GameData.CharacterData.Count; i++)
       {
           GameData.CharacterData[i].DeselectItem();
       }
   }
   
   public void DeselectAllGuns()
   {
       for (int i = 0; i < GameData.GunsData.Count; i++)
       {
           GameData.GunsData[i].DeselectItem();
       }
   }
}

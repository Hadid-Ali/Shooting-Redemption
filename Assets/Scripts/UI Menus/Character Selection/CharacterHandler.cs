using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*[Serializable]
public class CharacterStats
{
    [Range(0,100)] public  float Aim;
    [Range(0,100)] public float ReloadTime;
    [Range(0,100)] public float Bullets;
}*/
public class CharacterHandler : ItemHandler
{
//     public List<CharacterType> CharacterTOSpawn;
//     [SerializeField] private List<CharacterData> CharacterDatas = new();
//     [SerializeField] private List<GameObject> Characters = new();
//     private void Start()
//     {
//         m_SelectedItemIndex =Dependencies.GameDataOperations.GetSelectedCharacterIndex();
//         
//         m_Buy.onClick.AddListener(BuyCharacter);
//         m_Select.onClick.AddListener(SelectCharacter);
//         RetainGunData();
//         for (int i = 0; i < m_ItemSelectionButton.Count; i++)
//         {
//             var j = i;
//            // m_ItemSelectionButton[i].onClick.AddListener(() => SelectCharacter(j));
//         }
//         UpdateCoins();
//         UpdateCharacterData(m_SelectedItemIndex);
//     }
//
//     public void RetainGunData()
//     {
//         for (int i = 0; i < CharacterTOSpawn.Count; i++)
//         {
//             CharacterDatas.Add(ItemDataHandler.Instance.GetCharacterDataData(CharacterTOSpawn[i]));
//             GameObject Character = Instantiate(CharacterDatas[i].ItemPrefab);
//             Character.transform.SetParent(m_ParentObj);
//             Characters.Add(Character);
//             Character.SetActive(false);
//         }
//         
//         Characters[m_SelectedItemIndex].SetActive(true);
//         print(Characters[m_SelectedItemIndex].transform.name);
//     }
//
//     public void UpdateCharacterData(int CurrentIndex)
//     {
//         if (!Dependencies.GameDataOperations.GetCharacterUnlocked(CharacterDatas[CurrentIndex].character))
//         {
//             m_Buy.gameObject.SetActive(true);
//             m_Select.gameObject.SetActive(false);
//             m_ItemPrice.text = CharacterDatas[CurrentIndex].ItemPrice.ToString();
//         }
//         else
//         {
//             m_Buy.gameObject.SetActive(false);
//             m_Select.gameObject.SetActive(true);
//             if (Dependencies.GameDataOperations.GetSelectedCharacter(CharacterDatas[CurrentIndex].character))
//             {
//                 m_Select.gameObject.SetActive(false);
//                 m_Selected.gameObject.SetActive(true);
//             }
//         }
//     }
//
//
//     public void SelectCharacter(bool IsRight)
//     {
//         if (IsRight)
//         {
//             m_SelectedItemIndex++;
//         }
//         else
//         {
//             m_SelectedItemIndex--;
//         }
//         if (m_SelectedItemIndex >= Characters.Count || m_SelectedItemIndex < 0)
//         {
//             m_SelectedItemIndex = 0;
//         }
//         foreach (var character in Characters)
//             character.SetActive(false);
//         Characters[m_SelectedItemIndex].SetActive(true);
//         Dependencies.GameDataOperations.SetSelectedCharacterIndex((CharacterType) m_SelectedItemIndex);
//         UpdateCharacterData(m_SelectedItemIndex);
//         
//         /*if (CharacterIndex >= 0 && CharacterIndex < Characters.Count)
//         {
//             if (m_SelectedItemIndex != -1)
//             {
//                 Characters[m_SelectedItemIndex].SetActive(false);
//             }
//
//             Characters[CharacterIndex].SetActive(true);
//             m_SelectedItemIndex = CharacterIndex;
//
//             UpdateCharacterData(CharacterIndex);
//
//         }*/
//     }
//
//     void SelectCharacter()
//     {
//         print("selected");
//         Dependencies.GameDataOperations.DeselectAllCharacters();
//         Dependencies.GameDataOperations.SetSelectedCharacter(CharacterDatas[m_SelectedItemIndex].character);
//         UpdateCharacterData(m_SelectedItemIndex);
//         Dependencies.GameDataOperations.SetSelectedCharacterIndex((CharacterType) m_SelectedItemIndex);
//         Dependencies.GameDataOperations.SaveData();
//     }
//
//     /*
//     public void Deselect()
//     {
//         if (m_SelectedItemIndex != -1)
//         {
//             Characters[m_SelectedItemIndex].SetActive(false);
//             m_SelectedItemIndex = -1;
//         }
//     }
//     */
//
//     public void BuyCharacter()
//     {
//             int characterCost = CharacterDatas[m_SelectedItemIndex].ItemPrice;
//             if (Dependencies.GameDataOperations.SetCoins() >= characterCost)
//             {
//                 Dependencies.GameDataOperations.GetCoins(Dependencies.GameDataOperations.SetCoins() - characterCost);
//                 CharacterDatas[m_SelectedItemIndex].isLocked = false;
//                 Dependencies.GameDataOperations.SetCharacterUnlocked(CharacterDatas[m_SelectedItemIndex].character);
//                 UpdateCharacterData(m_SelectedItemIndex);
//                 print("buy gun");
//                 //SaveLoadData.SaveData();
//                 Dependencies.GameDataOperations.SaveData();
//                 UpdateCoins();
//
//             }
//             else
//             {
//                 print("not enough coins");
//             }
//     }
//
//     public void WatchAD()
//     {
//       //  SaveLoadData.GameData.m_Coins += 300;
//         Dependencies.GameDataOperations.GetCoins(300);
//         Dependencies.GameDataOperations.SaveData();
//         //SaveLoadData.SaveData();
//         UpdateCoins();
//     }
//
//     void UpdateCoins()
//     {
//         m_TotalCoins.text = Dependencies.GameDataOperations.SetCoins().ToString();
//     }
}

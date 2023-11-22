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
    public List<CharacterType> CharacterTOSpawn;
    [SerializeField] private List<CharacterData> CharacterDatas = new();
    [SerializeField] private List<GameObject> Characters = new();
    private void Start()
    {
        m_SelectedItemIndex = SaveLoadData.GameData.m_SelectedCharacterIndex;
        
        m_Buy.onClick.AddListener(BuyCharacter);
        m_Select.onClick.AddListener(SelectCharacter);
        RetainGunData();
        for (int i = 0; i < m_ItemSelectionButton.Count; i++)
        {
            var j = i;
            m_ItemSelectionButton[i].onClick.AddListener(() => SelectCharacter(j));
        }

        UpdateCharacterData(m_SelectedItemIndex);
    }

    public void RetainGunData()
    {
        for (int i = 0; i < CharacterTOSpawn.Count; i++)
        {
            CharacterDatas.Add(ItemDataHandler.Instance.GetCharacterDataData(CharacterTOSpawn[i]));
            GameObject Character = Instantiate(CharacterDatas[i].ItemPrefab);
            Character.transform.SetParent(m_ParentObj);
            Characters.Add(Character);
            Character.SetActive(false);
        }
        
        Characters[m_SelectedItemIndex].SetActive(true);
        print(Characters[m_SelectedItemIndex].transform.name);
    }

    public void UpdateCharacterData(int CurrentIndex)
    {
        if (!SaveLoadData.Instance.GetCharacterUnlocked(CharacterDatas[CurrentIndex].character))
        {
            m_Buy.gameObject.SetActive(true);
            m_Select.gameObject.SetActive(false);
            m_ItemPrice.text = CharacterDatas[CurrentIndex].ItemPrice.ToString();
        }
        else
        {
            m_Buy.gameObject.SetActive(false);
            m_Select.gameObject.SetActive(true);
            if (SaveLoadData.Instance.GetSelectedCharacter(CharacterDatas[CurrentIndex].character))
            {
                m_Select.gameObject.SetActive(false);
                m_Selected.gameObject.SetActive(true);
            }
        }
    }


    public void SelectCharacter(int CharacterIndex)
    {
        if (CharacterIndex >= 0 && CharacterIndex < Characters.Count)
        {
            if (m_SelectedItemIndex != -1)
            {
                Characters[m_SelectedItemIndex].SetActive(false);
            }

            Characters[CharacterIndex].SetActive(true);
            m_SelectedItemIndex = CharacterIndex;
            
            UpdateCharacterData(CharacterIndex);

        }
    }

    void SelectCharacter()
    {
        print("selected");
        SaveLoadData.Instance.DeselectAllCharacters();
        SaveLoadData.Instance.SetSelectedCharacter(CharacterDatas[m_SelectedItemIndex].character);
        UpdateCharacterData(m_SelectedItemIndex);
        SaveLoadData.GameData.m_SelectedCharacterIndex = m_SelectedItemIndex;
        SaveLoadData.SaveData();
    }

    /*
    public void Deselect()
    {
        if (m_SelectedItemIndex != -1)
        {
            Characters[m_SelectedItemIndex].SetActive(false);
            m_SelectedItemIndex = -1;
        }
    }
    */

    public void BuyCharacter()
    {
            int characterCost = CharacterDatas[m_SelectedItemIndex].ItemPrice;
            if (SaveLoadData.GameData.m_Coins >= characterCost)
            {
                SaveLoadData.GameData.m_Coins -= characterCost;
                CharacterDatas[m_SelectedItemIndex].isLocked = false;
                SaveLoadData.Instance.SetCharacterUnlocked(CharacterDatas[m_SelectedItemIndex].character);
                UpdateCharacterData(m_SelectedItemIndex);
                print("buy gun");
                SaveLoadData.SaveData();
                UpdateCoins();

            }
            else
            {
                print("not enough coins");
            }
    }

    public void WatchAD()
    {
        SaveLoadData.GameData.m_Coins += 300;
        SaveLoadData.SaveData();
        UpdateCoins();
    }

    void UpdateCoins()
    {
        m_TotalCoins.text = SaveLoadData.GameData.m_Coins.ToString();
    }
}

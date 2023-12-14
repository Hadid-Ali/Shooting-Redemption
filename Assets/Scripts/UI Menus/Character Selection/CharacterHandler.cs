using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class CharacterSelectionData
{
    public GameObject characterPrefab;
    public CharacterStatus characterData;
}
public class CharacterHandler : MonoBehaviour
{
    [SerializeField]private List<CharacterSelectionData> characters = new List<CharacterSelectionData>();
    
    //Hard References
    public GameObject Camera;
    public Transform parentObj;
    public Button m_WatchAdForCoins;
    public Button m_WatchAdForFreeGunTry;
    public Button m_BuyButton;
    public Button m_SelectButton;
    public Button m_LeftScrollButton;
    public Button m_RightScrollButton;
    
    public TextMeshProUGUI m_GunButtonText;
    public Image GunStatus;
    
    public TextMeshProUGUI m_GunPrice;
    public TextMeshProUGUI m_Coins;

    
    //Logic
    private bool charactersAlreadyInstatiated;
    private int currentIndex;
    private CharacterType selectedCharacter;
    private CharacterType currentCharacter;
    
    
    private void OnEnable()
    {
        Camera.SetActive(true);
    }

    private void OnDisable()
    {
        Camera.SetActive(false);
    }
    private void Start()
    {
        charactersAlreadyInstatiated = false;
        
        selectedCharacter = Dependencies.GameDataOperations.GetSelectedCharacter();
        m_BuyButton.onClick.AddListener(BuyGun);
        m_SelectButton.onClick.AddListener(SelectGun);
        m_WatchAdForFreeGunTry.onClick.AddListener(OnClickRewardedAdGunFree);
        m_WatchAdForCoins.onClick.AddListener(OnClickRewardedAdCoins);
        m_LeftScrollButton.onClick.AddListener((() => ScrollGun(false)));
        m_RightScrollButton.onClick.AddListener((() => ScrollGun(true)));
        
        RetainGunData();
        UpdateGunData();
    }

    public void SyncData()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            Dependencies.GameDataOperations.SetCharacterData(characters[i].characterData);
        }
        Dependencies.GameDataOperations.SetSelectedCharacter(selectedCharacter);
        
        Dependencies.GameDataOperations.SaveData();
    }

    public void RetainGunData()
    {
        selectedCharacter = Dependencies.GameDataOperations.GetSelectedCharacter();
        
        List<CharacterStatus> guns = Dependencies.GameDataOperations.GetAllCharactersData();
        for (int i = 0; i < guns.Count; i++)
        {
            characters[i].characterData = guns[i];

            if (!charactersAlreadyInstatiated)
            {
                GameObject gun = Instantiate(SessionData.Instance.GetCharacterData(guns[i].character).ItemPrefab, parentObj,
                    true);
                characters[i].characterPrefab = gun;
            }
            characters[i].characterPrefab.SetActive(false);

            if (characters[i].characterData.character == selectedCharacter)
            {
                currentIndex = i;
                currentCharacter = characters[i].characterData.character;
            }
        }
        characters.Find(x => x.characterData.character == selectedCharacter).characterPrefab.SetActive(true);
        UpdateGunData();
    }

    public void UpdateGunData()
    {
        //Data Assessing
        bool isGunUnlocked = characters[currentIndex].characterData.isUnlocked;
        bool isGunSelected = selectedCharacter == currentCharacter;
        bool isAffordable = Dependencies.GameDataOperations.GetCredits() >=
                            SessionData.Instance.GetCharacterData(currentCharacter).ItemPrice;

        //Assignation
        m_GunPrice.SetText("Price : " + SessionData.Instance.GetCharacterData(currentCharacter).ItemPrice);
        m_Coins.SetText(Dependencies.GameDataOperations.GetCredits().ToString());


        if (isGunUnlocked)
        {
            if (isGunSelected)
            {
                m_GunButtonText.SetText("Selected");
                m_GunPrice.SetText("");
                GunStatus.color = Color.green;
                m_BuyButton.interactable = false;
            }
            else
            {
                m_BuyButton.onClick.RemoveAllListeners();
                m_BuyButton.onClick.AddListener(SelectGun);
                m_BuyButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Select");
                m_BuyButton.interactable = true;

                m_GunButtonText.SetText("Not Selected");
                GunStatus.color = Color.white;
            }
            m_WatchAdForFreeGunTry.interactable = false;
        }
        else if (isGunSelected) //For Rewarded
        {
            m_GunButtonText.SetText("Trial");
            GunStatus.color = Color.cyan;
        }
        else
        {
            m_GunButtonText.SetText("Unlock Character"); //Gun Status
            GunStatus.color = Color.white; // Gun Button Color
            
            m_BuyButton.onClick.RemoveAllListeners();
            
            if (isAffordable)
            {
                m_BuyButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Buy");
                m_BuyButton.onClick.AddListener(BuyGun);
                m_BuyButton.interactable = true;
            }
            else
            {
                m_BuyButton.interactable = false;
            }
            m_WatchAdForFreeGunTry.interactable = true;
        }

    }


    public void ScrollGun(bool IsRight)
    {
        if (IsRight)
        {
            currentIndex++;
        }
        else
        {
            currentIndex--;
        }

        if (currentIndex >= characters.Count || currentIndex <= 0)
        {
            currentIndex = 0;
        }

        foreach (var v in characters)
            v.characterPrefab.SetActive(false);
        
        characters[currentIndex].characterPrefab.SetActive(true);

        currentCharacter = characters[currentIndex].characterData.character;
        UpdateGunData();
    }

    public void SelectGun()
    {
        selectedCharacter = currentCharacter;
        SyncData();
        UpdateGunData();
    }
    public void BuyGun()
    {
        int gunCost = SessionData.Instance.GetCharacterData(selectedCharacter).ItemPrice;
        int availableCredits = Dependencies.GameDataOperations.GetCredits();
        
        if (availableCredits >= gunCost && !characters[currentIndex].characterData.isUnlocked)
        {
            Dependencies.GameDataOperations.SetCredit(availableCredits - gunCost);
            characters[currentIndex].characterData.isUnlocked = true;
            SyncData();
            UpdateGunData();
            Dependencies.GameDataOperations.SaveData();
        }
    }

    public void OnClickRewardedAdGunFree()
    {
        AdHandler.ShowRewarded(OnRewardedGunADWatched);
    }

    public void OnClickRewardedAdCoins()
    {
        AdHandler.ShowRewarded(OnRewardedCoinsAdWatched);
    }

    public void OnRewardedCoinsAdWatched()
    {
        Dependencies.GameDataOperations.SetCredit(Dependencies.GameDataOperations.GetCredits() + 300);
        Dependencies.GameDataOperations.SaveData();
        
        m_Coins.SetText(Dependencies.GameDataOperations.GetCredits().ToString());
    }
    public void OnRewardedGunADWatched()
    {
        Dependencies.GameDataOperations.SetSelectedCharacter(currentCharacter);
        selectedCharacter = currentCharacter;
        UpdateGunData();
        Dependencies.GameDataOperations.SaveData();
    }
}

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
    [SerializeField] private List<CharacterSelectionData> weapons = new List<CharacterSelectionData>();
    
    
    //Hard References
    public GameObject Camera;
    
    public Button m_WatchAdForCoins;
    public Button m_WatchAdForFreeGunTry;
    public Button m_BuyButton;
    public Button m_SelectButton;
    public Button m_LeftScrollButton;
    public Button m_RightScrollButton;
    
    
    public Image GunStatus;
    public TextMeshProUGUI m_GunPrice;
    public TextMeshProUGUI m_Coins;

    
    //Logic
    private bool gunsAlreadyInstatiated;
    private int currentIndex;
    private CharacterType selectedWeapon;
    private CharacterType currentWeapon;
    
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
        gunsAlreadyInstatiated = false;
        
        selectedWeapon = Dependencies.GameDataOperations.GetSelectedCharacter();
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
        for (int i = 0; i < weapons.Count; i++)
        {
            Dependencies.GameDataOperations.SetCharacterData(weapons[i].characterData);
        }
        Dependencies.GameDataOperations.SetSelectedCharacter(selectedWeapon);
        
        Dependencies.GameDataOperations.SaveData();
    }

    public void RetainGunData()
    {
        selectedWeapon = Dependencies.GameDataOperations.GetSelectedCharacter();
        
        List<CharacterStatus> guns = Dependencies.GameDataOperations.GetAllCharactersData();
        for (int i = 0; i < guns.Count; i++)
        {
            weapons[i].characterData = guns[i];

            if (!gunsAlreadyInstatiated)
            {
                GameObject gun = Instantiate(SessionData.Instance.GetCharacterData(guns[i].character).ItemPrefab);
                weapons[i].characterPrefab = gun;
            }
            weapons[i].characterPrefab.SetActive(false);

            if (weapons[i].characterData.character == selectedWeapon)
            {
                currentIndex = i;
                currentWeapon = weapons[i].characterData.character;
            }
        }
        weapons.Find(x => x.characterData.character == selectedWeapon).characterPrefab.SetActive(true);

        UpdateGunData();
    }
    

    public void UpdateGunData()
    {
        //Data Assessing
        bool isGunUnlocked = weapons[currentIndex].characterData.isUnlocked;
        bool isGunSelected = selectedWeapon == currentWeapon;
        
        int price = SessionData.Instance.GetCharacterData(currentWeapon).ItemPrice;
        int availableCoins = Dependencies.GameDataOperations.GetCredits();
        
        bool isAffordable = availableCoins >= price;

        m_BuyButton.interactable = !isGunUnlocked && isAffordable;
        m_SelectButton.interactable = isGunUnlocked && !isGunSelected;
        m_WatchAdForFreeGunTry.interactable = !isGunUnlocked;
        GunStatus.gameObject.SetActive(isGunSelected);
        m_GunPrice.SetText(isGunSelected ? "" : price.ToString());
        m_Coins.SetText(availableCoins.ToString());
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

        if (currentIndex >= weapons.Count || currentIndex <= 0)
        {
            currentIndex = 0;
        }

        foreach (var v in weapons)
            v.characterPrefab.SetActive(false);
        
        weapons[currentIndex].characterPrefab.SetActive(true);

        currentWeapon = weapons[currentIndex].characterData.character;
        UpdateGunData();
    }
    public void SelectGun()
    {
        selectedWeapon = currentWeapon;
        SyncData();
        UpdateGunData();
    }
    public void BuyGun()
    {
        int gunCost = SessionData.Instance.GetCharacterData(currentWeapon).ItemPrice;
        int availableCredits = Dependencies.GameDataOperations.GetCredits();
        
        if (availableCredits >= gunCost && !weapons[currentIndex].characterData.isUnlocked)
        {
            Dependencies.GameDataOperations.SetCredit(availableCredits - gunCost);
            weapons[currentIndex].characterData.isUnlocked = true;
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
        Dependencies.GameDataOperations.SetSelectedCharacter(currentWeapon);
        selectedWeapon = currentWeapon;
        UpdateGunData();
        Dependencies.GameDataOperations.SaveData();
    }


}

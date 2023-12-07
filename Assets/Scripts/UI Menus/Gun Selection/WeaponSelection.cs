using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

[Serializable]
public class WeaponSelectionData
{
    public GameObject weaponPrefab;
    public GunStatus weaponData;
}
public class WeaponSelection : MonoBehaviour
{
    [SerializeField] private List<WeaponSelectionData> weapons = new List<WeaponSelectionData>();
    
    
    //Hard References
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
    private bool gunsAlreadyInstatiated;
    private int currentIndex;
    private OverlayWeapons selectedWeapon;
    private OverlayWeapons currentWeapon;
    
    
    private void Start()
    {
        gunsAlreadyInstatiated = false;
        
        selectedWeapon = Dependencies.GameDataOperations.GetSelectedWeapon();
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
            Dependencies.GameDataOperations.SetGunData(weapons[i].weaponData);
        }
        Dependencies.GameDataOperations.SetSelectedWeapon(selectedWeapon);
        
        Dependencies.GameDataOperations.SaveData();
    }

    public void RetainGunData()
    {
        selectedWeapon = Dependencies.GameDataOperations.GetSelectedWeapon();
        
        List<GunStatus> guns = Dependencies.GameDataOperations.GetAllWeaponsData();
        for (int i = 0; i < guns.Count; i++)
        {
            weapons[i].weaponData = guns[i];

            if (!gunsAlreadyInstatiated)
            {
                GameObject gun = Instantiate(SessionData.Instance.GetGunData(guns[i].weapon).ItemPrefab, parentObj,
                    true);
                weapons[i].weaponPrefab = gun;
            }
            weapons[i].weaponPrefab.SetActive(false);

            if (weapons[i].weaponData.weapon == selectedWeapon)
            {
                currentIndex = i;
                currentWeapon = weapons[i].weaponData.weapon;
            }
        }
        weapons.Find(x => x.weaponData.weapon == selectedWeapon).weaponPrefab.SetActive(true);
        
    }

    public void UpdateGunData()
    {
        //Data Assessing
        bool isGunUnlocked = weapons[currentIndex].weaponData.isUnlocked;
        bool isGunSelected = selectedWeapon == currentWeapon;
        bool isAffordable = Dependencies.GameDataOperations.GetCredits() >=
                            SessionData.Instance.GetGunData(selectedWeapon).ItemPrice;

        //Assignation
        m_GunPrice.SetText("Price : " + SessionData.Instance.GetGunData(selectedWeapon).ItemPrice);
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
        }
        else if (isGunSelected) //For Rewarded
        {
            m_GunButtonText.SetText("Trial");
            GunStatus.color = Color.cyan;
        }
        else
        {
            m_GunButtonText.SetText("Unlock Gun"); //Gun Status
            GunStatus.color = Color.white; // Gun Button Color

            m_BuyButton.onClick.RemoveAllListeners();
            m_BuyButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Buy");
            m_BuyButton.onClick.AddListener(BuyGun);
            m_BuyButton.interactable = true;
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

        if (currentIndex >= weapons.Count || currentIndex <= 0)
        {
            currentIndex = 0;
        }

        foreach (var v in weapons)
            v.weaponPrefab.SetActive(false);
        
        weapons[currentIndex].weaponPrefab.SetActive(true);

        currentWeapon = weapons[currentIndex].weaponData.weapon;
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
        int gunCost = SessionData.Instance.GetGunData(selectedWeapon).ItemPrice;
        int availableCredits = Dependencies.GameDataOperations.GetCredits();
        
        if (availableCredits >= gunCost && !weapons[currentIndex].weaponData.isUnlocked)
        {
            Dependencies.GameDataOperations.SetCredit(availableCredits - gunCost);
            weapons[currentIndex].weaponData.isUnlocked = true;
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
        Dependencies.GameDataOperations.SetSelectedWeapon(currentWeapon);
        UpdateGunData();
        Dependencies.GameDataOperations.SaveData();
    }


    
    
}

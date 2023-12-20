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
    private OverlayWeapons selectedWeapon;
    private OverlayWeapons currentWeapon;


    private Mask mask;
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
                GameObject gun = Instantiate(SessionData.Instance.GetGunData(guns[i].weapon).ItemPrefab);
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

        UpdateGunData();
    }
    

    public void UpdateGunData()
    {
        //Data Assessing
        bool isGunUnlocked = weapons[currentIndex].weaponData.isUnlocked;
        bool isGunSelected = selectedWeapon == currentWeapon;
        
        int price = SessionData.Instance.GetGunData(currentWeapon).ItemPrice;
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
        int gunCost = SessionData.Instance.GetGunData(currentWeapon).ItemPrice;
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
        selectedWeapon = currentWeapon;
        UpdateGunData();
        Dependencies.GameDataOperations.SaveData();
    }
}

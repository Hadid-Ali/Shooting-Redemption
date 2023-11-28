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
    public OverlayWeapons weapon;
    public GameObject Prefab;
    public bool isUnlocked;
    public bool isSelected;
}
public class WeaponSelection : MonoBehaviour
{
    [SerializeField] private List<WeaponSelectionData> weapons = new List<WeaponSelectionData>();
    
    public OverlayWeapons selectedWeapon;
    public int currentIndex;
    
    public Transform parentObj;

    public Button m_WatchAdForCoins;
    public Button m_WatchAdForFreeGunTry;
    public Button m_BuyButton;
    
    public TextMeshProUGUI m_GunButtonText;
    
    public Image GunStatus;
    
    public TextMeshProUGUI m_GunPrice;
    public TextMeshProUGUI m_Coins;
    
    private void Start()
    {
        selectedWeapon = Dependencies.GameDataOperations.GetSelectedWeapon();
        m_BuyButton.onClick.AddListener(BuyGun);
        
        RetainGunData();
        UpdateGunData();
    }
    

    public void RetainGunData()
    {
        Dictionary<OverlayWeapons, bool> _weapons = Dependencies.GameDataOperations.GetAllWeapons();

        foreach (var v in _weapons)
        {
            GameObject gun = Instantiate(ItemDataHandler.Instance.GetGunData(v.Key).ItemPrefab, parentObj, true);

            WeaponSelectionData data = new WeaponSelectionData();
            data.weapon = v.Key;
            data.Prefab = gun;
            data.isUnlocked = v.Value;
            data.isSelected = Dependencies.GameDataOperations.GetSelectedWeapon() == v.Key;
            
            weapons.Add(data); //WeaponData
            gun.SetActive(false);
        }

        weapons.Find(x => x.weapon == selectedWeapon).Prefab.SetActive(true);
    }

    public void UpdateGunData()
    {
        //Data Assessing
        bool isGunUnlocked = weapons[currentIndex].isUnlocked;
        bool isGunSelected = weapons[currentIndex].isSelected;
        bool isAffordable = Dependencies.GameDataOperations.GetCredits() >= ItemDataHandler.Instance.GetGunData(selectedWeapon).ItemPrice;
        
        //Assignation
        m_GunPrice.SetText( "Price : " + ItemDataHandler.Instance.GetGunData(selectedWeapon).ItemPrice);
        m_Coins.SetText(Dependencies.GameDataOperations.GetCredits().ToString());
            

        if (!isGunUnlocked)
        {
            m_GunButtonText.SetText("Unlock Gun"); //Gun Status
            GunStatus.color = Color.white; // Gun Button Color
            
            m_BuyButton.onClick.RemoveAllListeners();
            m_BuyButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Buy");
            m_BuyButton.onClick.AddListener(BuyGun);
            m_BuyButton.interactable = true;
            
        }
        else if (!isGunSelected)
        {
            m_BuyButton.onClick.RemoveAllListeners();
            m_BuyButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Select");
            //m_BuyButton.onClick.AddListener(ScrollGun);
            m_BuyButton.interactable = true;
            
            m_GunButtonText.SetText("Not Selected");
            GunStatus.color = Color.white;
        }
        else
        {
            m_GunButtonText.SetText("Selected");
            GunStatus.color = Color.green;
            m_BuyButton.interactable = false;
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
            v.Prefab.SetActive(false);
        
        weapons[currentIndex].Prefab.SetActive(true);
        
        
        Dependencies.GameDataOperations.SetSelectedWeapon(weapons[currentIndex].weapon);
        UpdateGunData();
    }
    
    
    public void BuyGun()
    {
        int gunCost = ItemDataHandler.Instance.GetGunData(selectedWeapon).ItemPrice;
        if (Dependencies.GameDataOperations.GetCredits() >= gunCost)
        {
            Dependencies.GameDataOperations.SetCredit(gunCost);
            Dependencies.GameDataOperations.SetGunData(selectedWeapon, true);
            UpdateGunData();

            Dependencies.GameDataOperations.SaveData();
        }
    }

    public void OnClickRewardedAdGunFree()
    {
        GameAdEvents.ShowRewardedAd.Raise(OnRewardedGunADWatched);
    }

    public void OnClickRewardedAdCoins()
    {
        GameAdEvents.ShowRewardedAd.Raise(OnRewardedCoinsAdWatched);
    }

    public void OnRewardedCoinsAdWatched()
    {
        Dependencies.GameDataOperations.SetCredit(Dependencies.GameDataOperations.GetCredits() + 300);
        Dependencies.GameDataOperations.SaveData();
    }
    public void OnRewardedGunADWatched()
    {
        Dependencies.GameDataOperations.SetSelectedWeapon(selectedWeapon);
    }


    
    
}

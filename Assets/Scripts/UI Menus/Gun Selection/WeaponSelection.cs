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
    
    public Image GunStatus;
    public TextMeshProUGUI m_GunPrice;

    
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

        GameEvents.GamePlayEvents.updateButton.Raise(ButtonType.Buy, !isGunUnlocked && isAffordable); 
        GameEvents.GamePlayEvents.updateButton.Raise(ButtonType.Select, isGunUnlocked && !isGunSelected); 
        GameEvents.GamePlayEvents.updateButton.Raise(ButtonType.TryForFree, !isGunUnlocked); 

        GunStatus.gameObject.SetActive(isGunSelected);
        m_GunPrice.SetText(isGunSelected ? "" : price.ToString());
        m_GunPrice.transform.parent.gameObject.SetActive(!isGunUnlocked);
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

        if (currentIndex >= weapons.Count || currentIndex < 0)
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
            Dependencies.GameDataOperations.SubtractCredits(availableCredits - gunCost);
            weapons[currentIndex].weaponData.isUnlocked = true;
            SyncData();
            UpdateGunData();
            Dependencies.GameDataOperations.SaveData();
        }
    }
    public void OnRewardedGunADWatched()
    {
        Dependencies.GameDataOperations.SetSelectedWeapon(currentWeapon);
        selectedWeapon = currentWeapon;
        UpdateGunData();
        Dependencies.GameDataOperations.SaveData();
    }
}

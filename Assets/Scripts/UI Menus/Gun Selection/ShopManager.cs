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
public class GunsStats
{
    [Range(0,100)] public  float Aim;
    [Range(0,100)] public float ReloadTime;
    [Range(0,100)] public float Bullets;
}

public class ShopManager : MonoBehaviour
{
    public List<GunName> gunsTOSpawn;
    [SerializeField]private List<GunData> GunDatas = new();
    private List<GameObject> Guns = new();

    public int defaultGunIndex = 0;
    private int selectedGunIndex = 0;
    public Transform parentObj;

    public Button m_WatchAdBtn;

    public Button m_GunButton;
    public TextMeshProUGUI m_GunButtonText;

    public Image m_GunPriceContainer;
    
    public TextMeshProUGUI m_GunPrice;
    public TextMeshProUGUI m_Coins;

    [SerializeField] private List<GunsStats> m_GunsStats;


    private void Start()
    {
        selectedGunIndex = Dependencies.GameDataOperations.GetSelectedGunIndex();
        m_WatchAdBtn.onClick.AddListener(OnRewardedADWatched);
        
        RetainGunData();
        UpdateGunData(selectedGunIndex);
    }

    public void RetainGunData()
    {
        for (int i = 0; i < gunsTOSpawn.Count; i++)
        {
            GunDatas.Add(ItemDataHandler.Instance.GetGunData(gunsTOSpawn[i]));
            GameObject gun = Instantiate(GunDatas[i].ItemPrefab);
            gun.transform.SetParent(parentObj);
            Guns.Add(gun);
            gun.SetActive(false);
        }

        Guns[selectedGunIndex].SetActive(true);
    }

    public void UpdateGunData(int CurrentIndex)
    {
        m_GunButton.onClick.RemoveAllListeners();
        
        //Data Assessing
        bool isGunUnlocked = Dependencies.GameDataOperations.GetGunUnlocked(GunDatas[CurrentIndex].gun);
        bool isGunSelected = Dependencies.GameDataOperations.GetSelectedGun(GunDatas[CurrentIndex].gun);
        bool isAffordable = Dependencies.GameDataOperations.GetCoins() >= GunDatas[CurrentIndex].ItemPrice;
        
        //Assignation
        m_GunPrice.SetText( "Price : " + GunDatas[CurrentIndex].ItemPrice.ToString());
        m_Coins.SetText(Dependencies.GameDataOperations.GetCoins().ToString());
            

        if (!isGunUnlocked)
        {
            m_GunButton.onClick.AddListener(BuyGun);
            m_GunButtonText.SetText("Unlock Gun"); //Gun Status
            m_GunPriceContainer.color =  isAffordable ? Color.green : Color.red ; //Price container Color
            m_GunButton.image.color = Color.white; // Gun Button Color
        }
        else if (!isGunSelected)
        {
            m_GunButton.onClick.AddListener(SelectGun);
            m_GunButtonText.SetText("Select Gun");
            m_GunPriceContainer.color =  Color.green ;
            m_GunButton.image.color = Color.white;
        }
        else
        {
            m_GunButton.onClick.RemoveAllListeners();
            m_GunButtonText.SetText("Selected");
            m_GunButton.image.color = Color.green;
            m_GunPriceContainer.color =  Color.green ;
        }
    }


    public void SelectGun(bool IsRight)
    {
        if (IsRight)
        {
            selectedGunIndex++;
        }
        else
        {
            selectedGunIndex--;
        }

        if (selectedGunIndex >= Guns.Count || selectedGunIndex <= 0)
        {
            selectedGunIndex = 0;
        }

        foreach (var gun in Guns)
            gun.SetActive(false);

        Guns[selectedGunIndex].SetActive(true);
        Dependencies.GameDataOperations.SetSelectedGunIndex(selectedGunIndex);
        UpdateGunData(selectedGunIndex);


        print(selectedGunIndex);
    }

    void SelectGun()
    {
        Dependencies.GameDataOperations.DeselectAllGuns();
        Dependencies.GameDataOperations.SetSelectedGun(GunDatas[selectedGunIndex].gun);
        UpdateGunData(selectedGunIndex);
        Dependencies.GameDataOperations.SetSelectedGunIndex(selectedGunIndex);
        Dependencies.GameDataOperations.SaveData();
    }

    public void Deselect()
    {
        if (selectedGunIndex != -1)
        {
            Guns[selectedGunIndex].SetActive(false);
            selectedGunIndex = -1;
        }
    }

    public void BuyGun()
    {
        int gunCost = GunDatas[selectedGunIndex].ItemPrice;
        if (Dependencies.GameDataOperations.GetCoins() >= gunCost)
        {
            Dependencies.GameDataOperations.SetCoins(gunCost);
            GunDatas[selectedGunIndex].isLocked = false;
            Dependencies.GameDataOperations.SetGunUnlocked(GunDatas[selectedGunIndex].gun);
            UpdateGunData(selectedGunIndex);
            print("buy gun");
            Dependencies.GameDataOperations.SaveData();


        }
        else
        {
            print("not enough coins");
        }
    }

    public void OnRewardedADWatched()
    {
        Dependencies.GameDataOperations.SetCoins(300);
        Dependencies.GameDataOperations.SaveData();
    }


    
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

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

    public List<Button> GunButton;

    public int defaultGunIndex = 0;

    /*public int coins = 0;*/
    private int selectedGunIndex = 0;
    public Transform parentObj;

    public Button m_Buy;
    public Button m_Select;
    public Button m_Selected;

    public Button m_WatchAdBtn;
    public Text m_GunPrice;
    public Text m_TodalCoins;

    [SerializeField] private List<GunsStats> m_GunsStats;
    [SerializeField] private Image Aim, RelodTime, BulletCount;

    private void Start()
    {
        selectedGunIndex = SaveLoadData.GameData.m_SelectedGunIndex;
        m_Buy.onClick.AddListener(BuyGun);
        m_Select.onClick.AddListener(SelectGun);
        m_WatchAdBtn.onClick.AddListener(WatchAD);
        RetainGunData();
        for (int i = 0; i < GunButton.Count; i++)
        {
            var j = i;
            GunButton[i].onClick.AddListener(() => SelectGun(j));
        }

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
        if (!SaveLoadData.Instance.GetGunUnlocked(GunDatas[CurrentIndex].gun))
        {
            m_Buy.gameObject.SetActive(true);
            m_Select.gameObject.SetActive(false);
            m_GunPrice.text = GunDatas[CurrentIndex].ItemPrice.ToString();
        }
        else
        {
            m_Buy.gameObject.SetActive(false);
            m_Select.gameObject.SetActive(true);
            if (SaveLoadData.Instance.GetSelectedGun(GunDatas[CurrentIndex].gun))
            {
                m_Select.gameObject.SetActive(false);
                m_Selected.gameObject.SetActive(true);
            }
        }

        UpdateStatus(CurrentIndex);
    }

    void UpdateStatus(int CurrentIndex)
    {
        Aim.fillAmount = m_GunsStats[CurrentIndex].Aim / 100;
        RelodTime.fillAmount = m_GunsStats[CurrentIndex].ReloadTime / 100;
        BulletCount.fillAmount = m_GunsStats[CurrentIndex].Bullets / 100;
    }

    public void SelectGun(int gunIndex)
    {
        if (gunIndex >= 0 && gunIndex < Guns.Count)
        {
            if (selectedGunIndex != -1)
            {
                Guns[selectedGunIndex].SetActive(false);
            }

            Guns[gunIndex].SetActive(true);
            selectedGunIndex = gunIndex;
            SaveLoadData.GameData.m_SelectedGunIndex = gunIndex;
            UpdateGunData(gunIndex);
        }
    }

    void SelectGun()
    {
        SaveLoadData.Instance.DeselectAllGuns();
        SaveLoadData.Instance.SetSelectedGun(GunDatas[selectedGunIndex].gun);
        UpdateGunData(selectedGunIndex);
        SaveLoadData.GameData.m_SelectedGunIndex = selectedGunIndex;
        SaveLoadData.SaveData();
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
        if (SaveLoadData.GameData.m_Coins >= gunCost)
        {
            SaveLoadData.GameData.m_Coins -= gunCost;
            GunDatas[selectedGunIndex].isLocked = false;
            SaveLoadData.Instance.SetGunUnlocked(GunDatas[selectedGunIndex].gun);
            UpdateGunData(selectedGunIndex);
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
        m_TodalCoins.text = SaveLoadData.GameData.m_Coins.ToString();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using TMPro;

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
    public TextMeshProUGUI m_GunPrice;
    public TextMeshProUGUI m_TodalCoins;

    [SerializeField] private List<GunsStats> m_GunsStats;
    [SerializeField] private Image Aim, RelodTime, BulletCount;

    private void Start()
    {
        selectedGunIndex = Dependencies.GameDataOperations.GetSelectedGunIndex();
        m_Buy.onClick.AddListener(BuyGun);
        m_Select.onClick.AddListener(SelectGun);
        m_WatchAdBtn.onClick.AddListener(WatchAD);
        RetainGunData();
        for (int i = 0; i < GunButton.Count; i++)
        {
            var j = i;
           // GunButton[i].onClick.AddListener(() => SelectGun(j));
        }

        UpdateCoins();
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

        if (!Dependencies.GameDataOperations.GetGunUnlocked(GunDatas[CurrentIndex].gun))
        {
            m_Buy.gameObject.SetActive(true);
            m_Select.gameObject.SetActive(false);
            m_GunPrice.text = GunDatas[CurrentIndex].ItemPrice.ToString();
        }
        else
        {
            m_Buy.gameObject.SetActive(false);
            m_Select.gameObject.SetActive(true);
            if (Dependencies.GameDataOperations.GetSelectedGun(GunDatas[CurrentIndex].gun))
            {
                m_Select.gameObject.SetActive(false);
                m_Selected.gameObject.SetActive(true);
            }
        }

      //  UpdateStatus(CurrentIndex);
    }

    /*void UpdateStatus(int CurrentIndex)
    {
        Aim.fillAmount = m_GunsStats[CurrentIndex].Aim / 100;
        RelodTime.fillAmount = m_GunsStats[CurrentIndex].ReloadTime / 100;
        BulletCount.fillAmount = m_GunsStats[CurrentIndex].Bullets / 100;
    }*/

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

        if (selectedGunIndex >= Guns.Count || selectedGunIndex < 0)
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
        if (Dependencies.GameDataOperations.SetCoins() >= gunCost)
        {
            Dependencies.GameDataOperations.GetCoins(gunCost);
            GunDatas[selectedGunIndex].isLocked = false;
            Dependencies.GameDataOperations.SetGunUnlocked(GunDatas[selectedGunIndex].gun);
            UpdateGunData(selectedGunIndex);
            print("buy gun");
            Dependencies.GameDataOperations.SaveData();
            UpdateCoins();

        }
        else
        {
            print("not enough coins");
        }
    }

    public void WatchAD()
    {
        Dependencies.GameDataOperations.GetCoins(300) ;
        Dependencies.GameDataOperations.SaveData();
        
    }

    void UpdateCoins()
    {
        m_TodalCoins.text = Dependencies.GameDataOperations.SetCoins().ToString();
    }
    
    
}

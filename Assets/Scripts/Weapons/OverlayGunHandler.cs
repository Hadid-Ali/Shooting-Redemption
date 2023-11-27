using System.Collections.Generic;
using System.Net.Mime;
using CoverShooter;
using UnityEngine;
using UnityEngine.UI;


public class OverlayGunHandler : MonoBehaviour
{
    [SerializeField] private List<OverlayGun> Guns;
    [SerializeField] private GameObject Aim;
    private OverlayWeapons _selectedGun;
    

    private Image _FadeImage;
    private void Awake()
    {
        PlayerInputt.OnZoom += OnZoom;
        PlayerInputt.OnUnZoom += OnUnzoom;
        PlayerInventory.OnGunChangeSuccessFul += SelectGun;

        _FadeImage = Aim.GetComponent<Image>();
        
    }

    private void OnDestroy()
    {
        PlayerInputt.OnZoom -= OnZoom;
        PlayerInputt.OnUnZoom -= OnUnzoom;
        PlayerInventory.OnGunChangeSuccessFul -= SelectGun;
    }


    private void SelectGun(OverlayWeapons gunIndex)
    {
        _selectedGun = gunIndex;
    }
    public void OnZoom()
    {
        foreach (var v in Guns)
            v.gameObject.SetActive(v.weaponType == _selectedGun);
        
        Aim.SetActive(true);
        _FadeImage.color = Color.black;
    }

    public void OnUnzoom()
    {
        foreach (var v in Guns)
            v.gameObject.SetActive(false);

        _FadeImage.color = Color.clear;
        Aim.SetActive(false);
        
    }
    
}


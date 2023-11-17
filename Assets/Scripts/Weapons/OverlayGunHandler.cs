using System.Net.Mime;
using CoverShooter;
using UnityEngine;
using UnityEngine.UI;


public class OverlayGunHandler : MonoBehaviour, ICharacterZoomListener
{
    [SerializeField] private GameObject[] Guns;
    [SerializeField] private GameObject Aim;
    private int _selectedGun;
    

    private Image _FadeImage;
    private void Awake()
    {
        PlayerInputt.OnZoom += OnZoom;
        PlayerInputt.OnUnZoom += OnUnzoom;
        PlayerInputt.OnGunChange += SelectGun;
        _selectedGun = 0;

        _FadeImage = Aim.GetComponent<Image>();
    }

    private void OnDestroy()
    {
        PlayerInputt.OnZoom -= OnZoom;
        PlayerInputt.OnUnZoom -= OnUnzoom;
        PlayerInputt.OnGunChange -= SelectGun;
    }


    private void SelectGun(int gunIndex)
    {
        if (gunIndex == 0)
            gunIndex = 1;
        
        _selectedGun = gunIndex - 1;
    }
    public void OnZoom()
    {
        for (int i = 0; i < Guns.Length; i++)
        {
            if(i == _selectedGun)
                Guns[i].SetActive(true);
            else
                Guns[i].SetActive(false);
        }
        
        Aim.SetActive(true);
        _FadeImage.color = Color.black;;
        
    }

    public void OnUnzoom()
    {
        foreach (var v in Guns)
            v.SetActive(false);

        _FadeImage.color = Color.clear;
        Aim.SetActive(false);
    }

    public void OnScope()
    {
        throw new System.NotImplementedException();
    }

    public void OnUnscope()
    {
        throw new System.NotImplementedException();
    }

    public void OnBulletLoadStart()
    {
        foreach (var v in Guns)
            v.SetActive(false);
    }
    
}


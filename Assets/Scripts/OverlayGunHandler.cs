using System;
using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using CoverShooter.AI;
using UnityEngine;
using Hit = CoverShooter.Hit;

public class OverlayGunHandler : MonoBehaviour, ICharacterZoomListener, IGunListener
{
    [SerializeField] private GameObject[] Guns;
    [SerializeField] private GameObject Aim;
    private int _selectedGun;

    public static Action OnCustomZoom;
    public static Action OnCustomUnZoom;
    private static readonly int InActive = Animator.StringToHash("InActive");
    private static readonly int Active = Animator.StringToHash("Active");

    private void Awake()
    {
        ThirdPersonInput.GunChanged.Register(SelectGun);
        _selectedGun = 0;
    }

    private void OnDestroy()
    {
        ThirdPersonInput.GunChanged.UnRegister(SelectGun);
    }


    private void SelectGun(int gunIndex)
    {
        if (gunIndex == 0)
            gunIndex = 1;
        
        _selectedGun = gunIndex - 1;
    }
    public void OnZoom()
    {
        if(CharacterStates.playerState != PlayerCustomStates.HoldingPosition)
         return;
        
        for (int i = 0; i < Guns.Length; i++)
        {
            if(i == _selectedGun)
                Guns[i].SetActive(true);
            else
                Guns[i].SetActive(false);

        }
        
        Aim.GetComponent<Animator>().SetTrigger(Active);
        Aim.SetActive(true);
        OnCustomZoom();
    }

    public void OnUnzoom()
    {
        foreach (var v in Guns)
            v.SetActive(false);

        Aim.GetComponent<Animator>().SetTrigger(InActive);
        Aim.SetActive(false);
        OnCustomUnZoom();
    }
    public void OnBulletLoadStart()
    {
        foreach (var v in Guns)
            v.SetActive(false);
    }

#region USELSS

    public void OnScope()
    {
        throw new NotImplementedException();
    }

    public void OnUnscope()
    {
        throw new NotImplementedException();
    }
public void OnEject()
{
    throw new NotImplementedException();
}

public void OnRechamber()
{
    throw new NotImplementedException();
}

public void OnPump()
{
    throw new NotImplementedException();
}

public void OnFire(float delay)
{
    throw new NotImplementedException();
}

public void OnEmptyFire()
{
    throw new NotImplementedException();
}

public void OnBulletLoad()
{
    throw new NotImplementedException();
}

public void OnFullyLoaded()
{
    throw new NotImplementedException();
}



public void OnPumpStart()
{
    throw new NotImplementedException();
}

public void OnMagazineLoadStart()
{
    throw new NotImplementedException();
}
#endregion


}


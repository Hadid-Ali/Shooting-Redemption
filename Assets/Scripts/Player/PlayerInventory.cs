using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomWeapon
{
    public GameObject weaponRight;
    public GameObject weaponLeft;
    public OverlayWeapons Weapon;
    public bool TwoHandedweapon;
    public bool isRifle;
}
public class PlayerInventory : MonoBehaviour
{
    //Events
    public static Action<OverlayWeapons> OnGunChangeSuccessFul;
    
    public List<CustomWeapon> weapons;
    
    private static readonly int Pistol = Animator.StringToHash("EquipPistol");
    private static readonly int Rifle = Animator.StringToHash("EquipRifle");

    private Animator _animator;
    private CustomWeapon _selectedWeapon;
    

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        PlayerInputt.OnGunChangeInput += EquipWeapon;
    }

    private void Start()
    {
        UpdateSelectedWeapon(Dependencies.GameDataOperations.GetSelectedWeapon());
        GameEvents.GamePlayEvents.OnPlayerReachedCover.Register(OnPlayerCoverReached);
        OnGunChangeSuccessFul(_selectedWeapon.Weapon);
    }
    private void OnDestroy()
    {
        PlayerInputt.OnGunChangeInput -= EquipWeapon;
        GameEvents.GamePlayEvents.OnPlayerReachedCover.Unregister(OnPlayerCoverReached);
    }

    private void UpdateSelectedWeapon(OverlayWeapons i)
    {
        _selectedWeapon = weapons.Find(x => x.Weapon == Dependencies.GameDataOperations.GetSelectedWeapon());
    }

    public void OnPlayerCoverReached()
    {
        PlayerInputt.CanTakeInput = true;
        DrawWeapon(Dependencies.GameDataOperations.GetSelectedWeapon());

        CustomCameraController.CameraStateChanged(CamState.Idle);
    }


    public void EquipWeapon()
    {
        if(_selectedWeapon.isRifle)
            _animator.SetTrigger(Rifle);
        else
            _animator.SetTrigger(Pistol);
    }

    public void OnEquipAnimation()
    {
        foreach (var v in weapons)
        {
            v.weaponRight.SetActive(false);
            if(v.weaponLeft) v.weaponLeft.SetActive(false);
        }
        
        _selectedWeapon.weaponRight.SetActive(true); 
        if(_selectedWeapon.weaponLeft) _selectedWeapon.weaponLeft.SetActive(false);
        
        OnGunChangeSuccessFul(_selectedWeapon.Weapon);
    }

    public void DrawWeapon(OverlayWeapons i)
    {
        UpdateSelectedWeapon(i);
        EquipWeapon();
    }

    public bool IsCurrentWeaponRifle()
    {
        return _selectedWeapon.isRifle;
    }
    
    
}

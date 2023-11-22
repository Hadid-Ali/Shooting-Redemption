using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomWeapons
{
    public GameObject weaponRight;
    public GameObject weaponLeft;
    public bool TwoHandedweapon;
    public bool isRifle;
}
public class PlayerInventory : MonoBehaviour
{
    public CustomWeapons[] weapons;
    public int selectedWeapon;  
    private Animator _animator;
    
    private static readonly int Pistol = Animator.StringToHash("EquipPistol");
    private static readonly int Rifle = Animator.StringToHash("EquipRifle");

    public static Action<int> OnGunChangeSuccessFul;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        //selectedWeapon = -1;
        PlayerInputt.OnGunChangeInput += EquipWeapon;
    }

    private void Start()
    {
        OnGunChangeSuccessFul(selectedWeapon);
    }

    private void OnDestroy()
    {
        PlayerInputt.OnGunChangeInput -= EquipWeapon;
    }

    public void EquipWeapon()
    {
        //selectedWeapon++;
        
       // if (selectedWeapon <= 0 || selectedWeapon >= weapons.Length)
          //  selectedWeapon = 0;
        
        if(weapons[selectedWeapon].isRifle)
            _animator.SetTrigger(Rifle);
        else
            _animator.SetTrigger(Pistol);
        
        

    }

    public void OnEquipAnimation()
    {
        for (int i = 0; i < weapons.Length; i++)
            weapons[i].weaponRight.SetActive(i == selectedWeapon);

        OnGunChangeSuccessFul(selectedWeapon);
    }

    public void DrawWeapon(int i)
    {
        selectedWeapon = i;
        EquipWeapon();
    }

    public bool IsCurrentWeaponRifle()
    {
        if (selectedWeapon <= 0 || selectedWeapon >= weapons.Length)
            selectedWeapon = 0;
        
        return weapons[selectedWeapon].isRifle;
    }
    
    
}

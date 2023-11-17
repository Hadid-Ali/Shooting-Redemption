using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject[] weapons;
    public int selectedWeapon;
    private Animator _animator;
    
    private static readonly int Pistol = Animator.StringToHash("EquipPistol");
    private static readonly int Rifle = Animator.StringToHash("EquipRifle");

    public static int guns;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        guns = weapons.Length;
        PlayerInputt.OnGunChange += EquipWeapon;
    }

    private void OnDestroy()
    {
        PlayerInputt.OnGunChange -= EquipWeapon;
    }

    public void EquipWeapon(int gunIndex)
    {
        selectedWeapon = gunIndex;
        
        if(gunIndex == 0)
            _animator.SetTrigger(Pistol);
        else if(gunIndex == 1)
            _animator.SetTrigger(Rifle);
    }

    public void OnEquipAnimation()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if(i == selectedWeapon)
                weapons[i].SetActive(true);
            else
                weapons[i].SetActive(false);
        }
    }
    
    
}

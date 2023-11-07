using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using CoverShooter;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Image Bg;
    public Image GreenHp;

    public float animationSpeed;

    private CharacterHealth _characterHealth;
    public Transform cameraPosition;

    private float currentHp;
    private float maxhp;
    private void Awake()
    {
        _characterHealth = GetComponent<CharacterHealth>();
        _characterHealth.customHurt += UpdateHealth;

        maxhp = _characterHealth.MaxHealth;
        currentHp = maxhp;

        cameraPosition = Camera.main.transform;
    }

    private void OnDestroy()
    {
        _characterHealth.customHurt += UpdateHealth;
    }

    private void UpdateHealth(float val)
    {
        currentHp = val;
        float percentage= currentHp / maxhp;
        GreenHp.fillAmount = percentage;
        
        print("workinggg");
    }

    private void Update()
    {
        Quaternion lookAtRotation = 
            Quaternion.LookRotation(new Vector3(cameraPosition.position.x - transform.position.x, 
            0f, cameraPosition.position.z - transform.position.z));
        
        Bg.transform.rotation = Quaternion.Euler(0f, lookAtRotation.eulerAngles.y, 0f);
        GreenHp.transform.rotation = Quaternion.Euler(0f, lookAtRotation.eulerAngles.y, 0f);

        float currentPercentage = currentHp / maxhp;

        if (currentPercentage <= Bg.fillAmount)
        {
            Bg.fillAmount -= animationSpeed * Time.deltaTime;
        }
    }

}

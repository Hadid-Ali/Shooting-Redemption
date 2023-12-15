using System;
using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image healthUi;
    private void Awake()
    {
        healthUi = GetComponent<Image>();
        CharacterHealth.OnHealthUIUpdate.Register(OnUiHealthUpdate);    
    }

    private void OnDestroy()
    {
        CharacterHealth.OnHealthUIUpdate.UnRegister(OnUiHealthUpdate);    
    }

    private void OnUiHealthUpdate(float hp)
    {
        if(hp < 0.3f)
            healthUi.color = Color.red;
            
        healthUi.fillAmount = hp;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonComponent : MonoBehaviour
{
    [SerializeField] private Button m_ButtonComponent;
    [SerializeField] private ButtonType buttonType;
    
    void Start()
    {
        OnValidate();
        m_ButtonComponent.onClick.AddListener(ButtonClickInternal);
        
        GameEvents.GamePlayEvents.updateButton.Register(UpdateButton);
    }

    private void OnDestroy()
    {
        GameEvents.GamePlayEvents.updateButton.UnRegister(UpdateButton);
    }

    private void UpdateButton( ButtonType _buttonType,bool interactable)
    {
        if (_buttonType == buttonType)
        {
            OnValidate();
            m_ButtonComponent.interactable = interactable;
        }
    }

    private void OnValidate()
    {
        if (m_ButtonComponent != null)
            return;

        m_ButtonComponent = GetComponent<Button>();
    }

    private void ButtonClickInternal()
    {
        Dependencies.SoundHandler.PlaySFXSound(SFX.ButtonClick);
        GameEvents.GamePlayEvents.mainMenuButtonTap.Raise(buttonType);
        print("Working");
    }
}

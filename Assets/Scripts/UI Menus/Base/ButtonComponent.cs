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

    private GameEvent m_OnButtonTap = new();
    
    void Start()
    {
        m_ButtonComponent.onClick.AddListener(ButtonClickInternal);
        
    }

    private void OnValidate()
    {
        if (m_ButtonComponent != null)
            return;

        m_ButtonComponent = GetComponent<Button>();
    }

    private void ButtonClickInternal()
    {
        m_OnButtonTap.Raise();
        Dependencies.SoundHandler.BtnClickSound(buttonType);
    }
    
    public void SubscribeClick(Action action)
    {
        m_OnButtonTap.Register(action);
    }

    public void UnSubscribeClick(Action action)
    {
        m_OnButtonTap.Unregister(action);
    }
}

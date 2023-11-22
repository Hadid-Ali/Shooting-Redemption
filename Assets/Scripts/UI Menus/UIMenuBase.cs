using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIMenuBase : MonoBehaviour
{
    [SerializeField] private MenuName m_MenuName;
    [SerializeField] private GameObject m_MenuContainer;

    private bool m_IsActive = false;
    
    public MenuName MenuName => m_MenuName;

    private void OnEnable()
    {
        m_IsActive = gameObject.activeSelf;
    }

    public void SetMenuActiveState(bool isActive)
    {
        if (m_IsActive == isActive)
            return;
        
        if (isActive)
        {
            OnMenuContainerEnable();
        }
        else
        {
            OnMenuContainerDisable();
        }
        m_IsActive = isActive;
        m_MenuContainer.SetActive(isActive);
    }

    protected virtual void OnMenuContainerEnable()
    {
        
                                                       }

    protected virtual void OnMenuContainerDisable()
    {
        
    }

    protected void ChangeMenuState(MenuName menuName)
    {
        GameEvents.MenuEvents.MenuStateSwitched.Raise(menuName);
    }
}

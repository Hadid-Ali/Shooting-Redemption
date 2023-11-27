using UnityEngine;
using System.Collections.Generic;

public abstract class UIMenuHandler : MonoBehaviour
{
    [SerializeField] private List<UIMenuBase> m_Menus;

    private MenuName m_CurrentMenuName;
    public UIMenuBase m_CurrentMenu;

    protected virtual void Start()
    {
        GameAdEvents.MenuEvents.MenuControllerInit.Raise();
    }

    protected void OnEnable()
    {
        InitializeEvents();
    }

    private void OnDisable()
    {
        DeInitializeEvents();
    }

    protected virtual void InitializeEvents()
    {
        GameAdEvents.MenuEvents.MenuStateSwitched.Register(OnMenuTransition);
    }

    protected virtual void DeInitializeEvents()
    {
        GameAdEvents.MenuEvents.MenuStateSwitched.UnRegister(OnMenuTransition);
    }
    
    protected void SetMenuState(MenuName menuName)
    {
        m_CurrentMenuName = menuName;
        HideAllMenus();

        if (menuName is MenuName.None)
            return;
        
        m_Menus.Find(x => x.MenuName == menuName).SetMenuActiveState(true);
        /*if (!m_Menus.Find(x => x.MenuName == menuName))
        {
            print( " kjlshdfjkshdjkfh");

        }
        m_CurrentMenu.SetMenuActiveState(true);*/
    }
   
    private void OnMenuTransition(MenuName menuName)
    {
        if (m_CurrentMenuName == menuName)
            return;
         
        SetMenuState(menuName);
    }
    
    private void HideAllMenus()
    {
        for (int i = 0; i < m_Menus.Count; i++)
        {
            m_Menus[i].SetMenuActiveState(false);
        }
    }

    protected void AddMenu(UIMenuBase Panel)
    {
        m_Menus.Add(Panel);
    }
}

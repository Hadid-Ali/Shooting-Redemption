using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;

public abstract class ItemHandler : MonoBehaviour
{
    [SerializeField] protected Button m_Buy;
    [SerializeField] protected Button m_Select;
    [SerializeField] protected Button m_Selected;

    [SerializeField] protected TextMeshProUGUI m_ItemPrice;
    [SerializeField] protected TextMeshProUGUI m_TotalCoins;

    [SerializeField] protected int m_SelectedItemIndex = 0;
    [SerializeField] protected Transform m_ParentObj;

    [SerializeField] protected List<Button> m_ItemSelectionButton;


   

    
}

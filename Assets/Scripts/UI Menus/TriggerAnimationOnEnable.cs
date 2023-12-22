using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerAnimationOnEnable : MonoBehaviour
{
    private Animator _aniamtor;
    public bool isGamePlay;

    private void Awake()
    {
        _aniamtor = GetComponent<Animator>();
        
        if(!isGamePlay)
            _aniamtor.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void OnEnable()
    {
        if(!isGamePlay)
            _aniamtor.SetTrigger("Open");
    }
}

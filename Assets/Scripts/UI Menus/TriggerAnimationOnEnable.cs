using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerAnimationOnEnable : MonoBehaviour
{
    private Animator _aniamtor;

    private void Awake()
    {
        _aniamtor = GetComponent<Animator>();
        _aniamtor.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void OnEnable()
    {
        _aniamtor.SetTrigger("Open");
    }
}

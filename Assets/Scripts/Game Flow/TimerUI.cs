using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        Timer.OnTimerUIUpdate.Register(UpdateTimerUI);
    }

    private void OnDestroy()
    {
        Timer.OnTimerUIUpdate.UnRegister(UpdateTimerUI);
    }

    private void UpdateTimerUI(string text) => _text.SetText(text);
}

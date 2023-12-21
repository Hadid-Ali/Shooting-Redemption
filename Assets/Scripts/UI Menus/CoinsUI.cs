using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinsUI : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private int _previousCoins;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        
        GameEvents.GamePlayEvents.OnUpdateCoins.Register(OnUpdateCoins);
        _previousCoins = Dependencies.GameDataOperations.GetCredits();
        
        OnUpdateCoins(_previousCoins);
    }

    private void OnDestroy()
    {
        GameEvents.GamePlayEvents.OnUpdateCoins.UnRegister(OnUpdateCoins);
    }

    private void OnUpdateCoins(int coins)
    {
        if(gameObject is { activeSelf: true, activeInHierarchy: true })
            StartCoroutine(StatsVisualSequence(coins));
    }
    
    IEnumerator StatsVisualSequence(int coins)
    {
        _text.SetText(_previousCoins.ToString());
        float timer = 0f;
        while (timer < 2)
        {
            float progress = timer / 2;
            int newCoins = Mathf.RoundToInt(Mathf.Lerp(_previousCoins, coins, progress));
            _text.SetText(newCoins.ToString());
            
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        _previousCoins = coins;
    }
}

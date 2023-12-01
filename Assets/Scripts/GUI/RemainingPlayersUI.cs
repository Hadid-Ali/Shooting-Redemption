using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using CoverShooter;
using TMPro;
using UnityEngine;

public class RemainingPlayersUI : MonoBehaviour
{
    private TextMeshProUGUI text;
    
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        GamePlayStatsManager.OnUIUpdate.Register(UpdateText);
    }

    private void OnDestroy()
    {
        GamePlayStatsManager.OnUIUpdate.UnRegister(UpdateText);
    }


    public void UpdateText(GameplayStats stats)
    {
        text.SetText(stats.RemainingEnemies.ToString() + "/" + stats.TotalEnemies.ToString());
        print("Working");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameWinStats
{
    public int EnemiesKilled;
    public int CiviliansKilled;
    public int coinsEarned;
    public int previousCoins;
}
public class GameWinStatsVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI civiliansText;
    [SerializeField] private TextMeshProUGUI enemiesText;

    private int _civiliansKilled;

    private bool _statsShown;

    private GameWinStats _winStats;

    private void Awake()
    {
        LevelWinPanel.OnUpdateStatsStart.Register(UpdateStats);
    }

    private void OnDestroy()
    {
        LevelWinPanel.OnUpdateStatsStart.UnRegister(UpdateStats);
    }

    private void UpdateStats(GameWinStats gameWinStats)
    {
        _winStats = gameWinStats;
        StartCoroutine(StatsVisualSequence());
    }

    IEnumerator StatsVisualSequence()
    {
        yield return new WaitForSecondsRealtime(1f);
        coinsText.SetText(_winStats.previousCoins.ToString());
        
        float timer = 0f;
        while (timer < 2)
        {
            float progress = timer / 2;
            int newCoins = Mathf.RoundToInt(Mathf.Lerp(_winStats.previousCoins, _winStats.coinsEarned, progress));
            int enemyKilled = Mathf.RoundToInt(Mathf.Lerp(0, _winStats.EnemiesKilled, progress));
            int civiliansKilled = Mathf.RoundToInt(Mathf.Lerp(0, _winStats.CiviliansKilled, progress));
            
            
            enemiesText.SetText("Enemies Killed : " + enemyKilled + "  +" + (_winStats.EnemiesKilled * 50));
            civiliansText.SetText("Civilians Killed : " + civiliansKilled + "  -" + (_winStats.CiviliansKilled * 20));
            coinsText.SetText(newCoins.ToString());
            
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        
    }
}

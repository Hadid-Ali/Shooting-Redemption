using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayStatsManager : MonoBehaviour
{
    public static GameEvent<GameplayStats> OnUIUpdate;
    
    private void Awake()
    {
        GameEvents.GamePlayEvents.OnEnemyKilled.Register(UpdateStats);
    }

    private void OnDestroy()
    {
        GameEvents.GamePlayEvents.OnEnemyKilled.UnRegister(UpdateStats);
    }

    public void UpdateStats(GameplayStats stats)
    {
        OnUIUpdate.Raise(stats);
    }
    
}

public class GameplayStats
{
    public int TotalEnemies;
    public int RemainingEnemies;
}

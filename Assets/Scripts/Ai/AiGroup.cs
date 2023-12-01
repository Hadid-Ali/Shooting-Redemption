using System;
using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class AiGroup : MonoBehaviour
{
    public GameObject CoverPosition;
    
    private GameEvent<AiGroup> m_OnGroupKilled = new();
    
    [SerializeField] private List<CharacterHealth> enemies;

    private static HashSet<CharacterHealth> AllEnemies = new();
    private static int totalEnemiesCount;
    private static int resurrectingIterations;

    public static void ResurrectEnemey(CharacterHealth h)
    {
        AllEnemies.Add(h);
    }
    
    public void Initialize(Action<AiGroup> onactionDie, int respawningIteration)
    {
        m_OnGroupKilled.Register(onactionDie);
        
        foreach (var v in enemies)
            v.Initialize(OnEnemykilled);
        
        AllEnemies.AddRange(enemies);
        resurrectingIterations = respawningIteration;
        totalEnemiesCount = AllEnemies.Count * resurrectingIterations;

        GameplayStats s = new GameplayStats();
        s.TotalEnemies = totalEnemiesCount;
        s.RemainingEnemies = totalEnemiesCount;
        GamePlayStatsManager.OnUIUpdate.Raise(s);
    }

    private void OnDestroy()
    {
        m_OnGroupKilled.UnRegisterAll();
        GameEvents.GamePlayEvents.OnEnemyResurrected.UnRegister(ResurrectEnemey);
    }
    public static int GetRemainingEnemiesCount() => AllEnemies.Count;


    public void OnEnemykilled(CharacterHealth h)
    {
        AllEnemies.Remove(h);
        enemies.Remove(h);

        GameplayStats stat = new();
        stat.TotalEnemies = totalEnemiesCount;
        stat.RemainingEnemies = AllEnemies.Count;
        
        GameEvents.GamePlayEvents.OnEnemyKilled.Raise(stat);
        
        if (enemies.Count <= 0)
            m_OnGroupKilled.Raise(this);
    }
    
    
}

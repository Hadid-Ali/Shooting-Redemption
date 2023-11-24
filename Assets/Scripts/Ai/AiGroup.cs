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
    private GameEvent<CharacterHealth> m_OnEnemyKilled = new();
    
    
    [SerializeField] private List<CharacterHealth> enemies;
    [SerializeField] private int deadCount;
    [SerializeField] private int totalCount;
    

    public static int TotalEnemiesCount { private set; get; }

    private void Start()
    {
        deadCount = 0;
        totalCount = enemies.Count;
        
        foreach (var v in enemies)
        {
            v.Initialize(OnEnemykilled);
        }
        
        TotalEnemiesCount += enemies.Count;

    }

    public void AddEnemy(CharacterHealth h)
    {
        enemies.Add(h);
        totalCount = enemies.Count;

        List<CharacterHealth> temp = new List<CharacterHealth>();
        
        
        foreach (var v in enemies)
        {
            v.UnInitialize();
            v.Initialize(OnEnemykilled);
            if (v == null) temp.Add(v);
        }

        foreach (var v in temp)
            enemies.Remove(v);
        
    }
    
    public void Initialize(Action<AiGroup> onactionDie, Action<CharacterHealth> OnEnemyDie)
    {
        m_OnGroupKilled.Register(onactionDie);
        m_OnEnemyKilled.Register(OnEnemyDie);
    }

    private void OnDestroy()
    {
        m_OnGroupKilled.UnRegisterAll();
        m_OnEnemyKilled.UnRegisterAll();
        EnemyPoolManager.OnEnemyResurrect.UnRegister(AddEnemy);
        
        TotalEnemiesCount = 0;
    }


    public void OnEnemykilled(CharacterHealth h)
    {
        enemies.Remove(h);

        deadCount++;
        
        //if(enemies.Count >= 1) //So it doesn't throw error on the last enemy
        m_OnEnemyKilled.Raise(h);
        
        if (enemies.Count <= 0)
            m_OnGroupKilled.Raise(this);
        
        
    }

    public bool CheckLastEnemy()
    {
        return enemies.Count == 1;
    }
    
    
}

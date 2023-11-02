using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoverShooter;
using UnityEngine;

public class AiGroup : MonoBehaviour
{
    public GameObject CoverPosition;
    
    private GameEvent<AiGroup> m_OnGroupKilled = new();
    private GameEvent m_OnEnemyKilled = new();
    
    
    [SerializeField] private List<CharacterHealth> enemies;
    [SerializeField] private int totalCount;

    private void Start()
    {
        totalCount = enemies.Count;

        foreach (var v in enemies)
        {
            v.Initialize(OnEnemykilled);
        }
    }

    public void Initialize(Action<AiGroup> onactionDie, Action OnEnemyDie)
    {
        m_OnGroupKilled.Register(onactionDie);
        m_OnEnemyKilled.Register(OnEnemyDie);
    }


    public void OnEnemykilled()
    {
        for (int i = 0; i < enemies.Count; i++)
            if(enemies[i].Health <= 0) 
                enemies.Remove(enemies[i]);
        
        if(enemies.Count > 1) //So it doesn't throw error on the last enemy
            m_OnEnemyKilled.Raise();
        
        if (enemies.Count <= 0) 
            m_OnGroupKilled.Raise(this);
    }

    public bool CheckLastEnemy()
    {
        return enemies.Count == 1;
    }
    
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using UnityEngine;

public class AiGroup : MonoBehaviour
{
    public GameObject CoverPosition;
    
    private GameEvent<AiGroup> m_OnGroupKilled = new();
    private GameEvent m_OnEnemyKilled = new();
    
    
    [SerializeField] private CharacterHealth[] enemies;
    [SerializeField] private int deadCount;
    [SerializeField] private int totalCount;

    private void Start()
    {
        deadCount = 0;
        totalCount = enemies.Length;

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
        deadCount++;
        
        if(!CheckLastEnemy()) //So it doesn't throw error on the last enemy
            m_OnEnemyKilled.Raise();
        
        if (deadCount >= totalCount)
        {
            m_OnGroupKilled.Raise(this);
        }
    }

    public bool CheckLastEnemy()
    {
        return totalCount - deadCount == 1;
    }
    
    
}

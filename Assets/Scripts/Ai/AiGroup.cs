using System;
using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using UnityEngine;

public class AiGroup : MonoBehaviour
{
    public GameObject CoverPosition;
    
    private GameEvent<AiGroup> m_OnGroupKilled = new();
    private GameEvent<CharacterHealth> m_OnEnemyKilled = new();
    
    
    [SerializeField] private List<CharacterHealth> enemies;
    [SerializeField] private int deadCount;
    [SerializeField] private int totalCount;

    private void Start()
    {
        deadCount = 0;
        totalCount = enemies.Count;

        foreach (var v in enemies)
        {
            v.Initialize(OnEnemykilled);
        }
    }

    public void Initialize(Action<AiGroup> onactionDie, Action<CharacterHealth> OnEnemyDie)
    {
        m_OnGroupKilled.Register(onactionDie);
        m_OnEnemyKilled.Register(OnEnemyDie);
    }

    private void OnDestroy()
    {
        m_OnGroupKilled = null;
        m_OnEnemyKilled = null;
    }


    public void OnEnemykilled(CharacterHealth h)
    {
        enemies.Remove(h);

        totalCount--;
        
        if(enemies.Count >= 1) //So it doesn't throw error on the last enemy
            m_OnEnemyKilled.Raise(h);


        if (enemies.Count <= 0)
            m_OnGroupKilled.Raise(this);
        

    }

    public bool CheckLastEnemy()
    {
        return enemies.Count == 1;
    }
    
    
}

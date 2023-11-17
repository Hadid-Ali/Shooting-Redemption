
using System;
using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using UnityEngine;

public class AIGroupsHandler : MonoBehaviour
{
    [SerializeField] private List<AiGroup> m_AIgroups;
    [SerializeField] private int groupsDown;
    [SerializeField] private int totalgroups;
    [SerializeField] private bool hasBoss;
    [SerializeField] private Transform playerStartPos;
    
    public static GameEvent AllGroupsCCleared = new ();
    public static GameEvent<bool> hasBossE = new ();
    public static GameEvent<Transform> SetPlayerStartPosition = new();
    

    public static bool isLastEnemy;

    private EnemyPoolManager _enemyPoolManager;
    [SerializeField] private int resurrectingIterations ;
    [SerializeField] private Transform enemyResurrectPosition;
    [SerializeField] private int enemyResurrectDelay;

    private int iterationCounter ;


    private void Awake()
    {
        _enemyPoolManager = GetComponent<EnemyPoolManager>();
        
        _enemyPoolManager.time = enemyResurrectDelay;
        _enemyPoolManager.spawnPosition = enemyResurrectPosition;

        iterationCounter = -1; //Because first time its called is wasted
    }

    private void  Start()
    {
        SetPlayerStartPosition.Raise(playerStartPos);
        
        hasBossE.Raise(hasBoss);

        isLastEnemy = false;
        
        groupsDown = 0;
        totalgroups = m_AIgroups.Count;

        foreach (var v in m_AIgroups)
        {
            v.Initialize(OnAreaCleared, CheckLastEnemy);
        }
        
        CutScene.CutSceneEnded.Register(EnableGroup);
        
        CheckLastEnemy(new CharacterHealth());
    }

    private void OnDestroy()
    {
        CutScene.CutSceneEnded.Unregister(EnableGroup);
    }

    private void CheckLastEnemy(CharacterHealth h)  //This checks for last enemy (if its the last group alive) 
    {
        if (m_AIgroups.Count == 1)
            isLastEnemy = m_AIgroups[0].CheckLastEnemy();

        if (iterationCounter <= AiGroup.TotalEnemiesCount * resurrectingIterations && iterationCounter >= 0)
        {
            _enemyPoolManager.ResurectEnemy(h);
            m_AIgroups[m_AIgroups.Count - 1].AddEnemy(h);
        }
        iterationCounter++;
            
    }

    private void OnAreaCleared(AiGroup aiGroup)
    {
        groupsDown++;
        m_AIgroups.Remove(aiGroup);
        
        if (m_AIgroups.Count < 1)
        {
            AllGroupsCCleared.Raise();
        }
        else
            EnableGroup();

    }
    private void EnableGroup()
    {
        EnemyGroupEvents.OnEnemyGroupKilled.Raise(m_AIgroups[0].CoverPosition.transform);
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIGroupsHandler : MonoBehaviour
{
    [SerializeField] private List<AiGroup> m_AIgroups;
    [SerializeField] private int groupsDown;
    [SerializeField] private int totalgroups;
    
    public static GameEvent AllGroupsCCleared = new ();

    public static bool isLastEnemy = false;


    
    private void Start()
    {
        groupsDown = 0;
        totalgroups = m_AIgroups.Count;

        foreach (var v in m_AIgroups)
        {
            v.Initialize(OnAreaCleared, CheckLastEnemy);
        }
        
        CutScene.CutSceneEnded.Register(EnableGroup);
        
    }

    private void OnDestroy()
    {
        CutScene.CutSceneEnded.Unregister(EnableGroup);
    }

    private void CheckLastEnemy()  //This checks for last enemy (if its the last group alive) 
    {
        if (m_AIgroups.Count <= 1)
            isLastEnemy = m_AIgroups[0].CheckLastEnemy();
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
        EnemyGroupEvents.OnEnemyGroupKilled.Raise(m_AIgroups[0].CoverPosition.transform.position);
    }
    
}

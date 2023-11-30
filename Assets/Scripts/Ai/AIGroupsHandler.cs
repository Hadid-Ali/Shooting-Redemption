
using System;
using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using UnityEngine;

public class AIGroupsHandler : MonoBehaviour
{
    [SerializeField] private List<AiGroup> m_AIgroups;
    [SerializeField] private bool hasBoss;
    public Transform playerStartPos;
    


    private EnemyPoolManager _enemyPoolManager;
    [SerializeField] [Range(1,5)] private int resurrectingIterations ;
    [SerializeField] private Transform[] enemyResurrectPositions;
    [SerializeField] private int enemyResurrectDelay;

    private bool hasShowBossOnce;
    private static Transform playerStartPosStatic ;


    private void Awake()
    {
        playerStartPosStatic = playerStartPos;
        _enemyPoolManager = GetComponent<EnemyPoolManager>();
        
        _enemyPoolManager.time = enemyResurrectDelay;
        _enemyPoolManager.spawnPosition = enemyResurrectPositions;
        hasShowBossOnce = false;
    }

    private void OnDestroy()
    {
        GameEvents.GamePlayEvents.OnPlayerReachedCover.Unregister(OnPlayerReachedCover);
    }
    
    public static Transform GetPlayerSpawnPoint()
    {
        return playerStartPosStatic;
    }

    private void OnPlayerReachedCover()
    {
        StartCoroutine(CutSceneSequence());
    }
    IEnumerator CutSceneSequence()
    {
        CharacterStates.playerState = PlayerCustomStates.CutScene;
        yield return new WaitForSeconds(1);
        if (hasBoss && !hasShowBossOnce)
        {
            hasShowBossOnce = true;
            GameEvents.GamePlayEvents.ShowingBoss.Raise(true);
            yield return new WaitForSeconds(3);
            GameEvents.GamePlayEvents.ShowingBoss.Raise(false);
        }
        CharacterStates.playerState = PlayerCustomStates.HoldingPosition;
        
    }
    
    private void OnAreaCleared(AiGroup aiGroup)
    {
        m_AIgroups.Remove(aiGroup);
        
        if (m_AIgroups.Count < 1)
            GameEvents.GamePlayEvents.OnAllGroupsCleared.Raise();
    }

    
}

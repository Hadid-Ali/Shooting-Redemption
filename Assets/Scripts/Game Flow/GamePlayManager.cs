using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private GameObject[] Levels;
    private PlayerInventory _playerInventory;

    private Transform playerSpawnPoint;

    private void Awake()
    {
        GameEvents.GamePlayEvents.OnAllGroupsCleared.Register(OnGameWon);
        
        LoadLevel(Dependencies.GameDataOperations.GetSelectedLevel());
        SpawnPlayer();
    }
    private void SpawnPlayer()
    {
        GameObject player =
            ItemDataHandler.Instance.GetPlayerPrefab(Dependencies.GameDataOperations.GetSelectedCharacter());
        
        GameObject g = Instantiate(player, playerSpawnPoint.position,playerSpawnPoint.rotation);
        
        GameEvents.GamePlayEvents.OnPlayerSpawned.Raise();
    }

    private void OnDestroy()    
    {
        GameEvents.GamePlayEvents.OnAllGroupsCleared.Unregister(OnGameWon);
    }
    
    
    public void LoadLevel(int i)
    {
        Levels[i].SetActive(true);
        playerSpawnPoint = Levels[i].GetComponentInChildren<AIGroupsHandler>().playerStartPos; //Get Player Start position
    }

    public void OnGameWon()
    {
        int currentLevel = Dependencies.GameDataOperations.GetSelectedLevel();
        int currentEpisode = Dependencies.GameDataOperations.GetSelectedEpisode();
        
        if (currentLevel < 4)
        {
            Dependencies.GameDataOperations.SetSelectedLevel(currentLevel++);
            Dependencies.GameDataOperations.SetUnlockedLevels(currentEpisode, currentLevel);

        }

        if (currentLevel >= 4)
            Dependencies.GameDataOperations.SetUnlockedEpisodes(currentEpisode);
    }
    
    

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    //[SerializeField] private GameObject[] Levels;
    private PlayerInventory _playerInventory;
    private Transform playerSpawnPoint;
    private void Awake()
    {
        Time.timeScale = 1;
        
      //  LoadLevel(Dependencies.GameDataOperations.GetSelectedLevel());
    }

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject player =
            SessionData.Instance.GetPlayerPrefab(Dependencies.GameDataOperations.GetSelectedCharacter());
        
        Instantiate(player, playerSpawnPoint.position,playerSpawnPoint.rotation);
        
        GameEvents.GamePlayEvents.OnPlayerSpawned.Raise();
    }
    
    public void LoadLevel(int i)
    {
        int levelToLoad = i + 1;
        int currentEpisode = Dependencies.GameDataOperations.GetSelectedEpisode();
        currentEpisode++;

        string path = "Levels/Episode" + currentEpisode + "/Level" + levelToLoad;
        GameObject g = Instantiate(Resources.Load<GameObject>(path));
        
        playerSpawnPoint = g.GetComponentInChildren<AIGroupsHandler>().playerStartPos; //Get Player Start position
    }
}

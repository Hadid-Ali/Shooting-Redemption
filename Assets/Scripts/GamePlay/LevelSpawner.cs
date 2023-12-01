using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] Levels;
    private PlayerInventory _playerInventory;
    private Transform playerSpawnPoint;
    private void Awake()
    {
        LoadLevel(Dependencies.GameDataOperations.GetSelectedLevel());
    }

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject player =
            ItemDataHandler.Instance.GetPlayerPrefab(Dependencies.GameDataOperations.GetSelectedCharacter());
        
        GameObject g = Instantiate(player, playerSpawnPoint.position,playerSpawnPoint.rotation);
        
        GameEvents.GamePlayEvents.OnPlayerSpawned.Raise();
    }
    
    public void LoadLevel(int i)
    {
        Levels[i].SetActive(true);
        playerSpawnPoint = Levels[i].GetComponentInChildren<AIGroupsHandler>().playerStartPos; //Get Player Start position
    }
}

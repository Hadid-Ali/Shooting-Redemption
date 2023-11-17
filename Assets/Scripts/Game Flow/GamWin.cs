using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamWin : MonoBehaviour
{
    [SerializeField] private GameObject _screen;
    [SerializeField] private GameObject[] Levels;
    
    public int selectedPlayer;
    public GameObject[] players;

    private void Awake()
    {
        AIGroupsHandler.AllGroupsCCleared.Register(OnGameWon);
        LoadLevel(PlayerPrefs.GetInt("SelectedLevel", 0));
        Instantiate(players[selectedPlayer]);
    }

    private void OnDestroy()    
    {
        AIGroupsHandler.AllGroupsCCleared.Unregister(OnGameWon);
    }

    public void OnGameWon()
    {
        StartCoroutine(wait());
    }

    public void OnLevelSelect(int i)
    {
        PlayerPrefs.SetInt("SelectedLevel", i);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex) ;
    }
    
    public void LoadLevel(int i)
    {
        Levels[i].SetActive(true);
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
        _screen.SetActive(true);
    }
    
    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

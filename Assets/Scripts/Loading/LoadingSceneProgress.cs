using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    Splash,
    MainMenu,
    LoadingScreen,
    Chapter1,
    Chapter2,
    Chapter3
}
public class LoadingSceneProgress : MonoBehaviour
{
    [SerializeField] private Image _loadingBar;
    
    private void Start()
    {
        String sceneName = Dependencies.GameDataOperations.GetSceneToLoadName().ToString();
        StartCoroutine(LoadAsyncScene(sceneName));
    }
    IEnumerator LoadAsyncScene(string m_LoadScene)
    {
        yield return new WaitForSeconds(.5f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_LoadScene);
        yield return new WaitForSeconds(.5f);
        while (!asyncLoad.isDone)
        {
            _loadingBar.fillAmount = asyncLoad.progress;
            yield return null;
        }
    }
}

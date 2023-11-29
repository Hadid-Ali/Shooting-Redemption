using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreenDuration : MonoBehaviour
{
    public float splashScreenDuration = 3f;
    public Image progressBar;
    public string sceneToLoad = "Main Menu";
    
    void Start()
    {
        StartCoroutine(LoadSceneAsync());
        AdHandler.InitializeAds();
    }
    
    IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(splashScreenDuration);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            progressBar.fillAmount = progress;
            yield return null;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreenDuration : MonoBehaviour
{
    public float splashScreenDuration = 3f;
    public Image progressBar;
    
    void Awake()
    {
        GameAdEvents.OnAdsInitialized.Register(OnAdsInitialized);
        
        AdHandler.InitializeAds();
        
    }

    private void OnDestroy()
    {
        GameAdEvents.OnAdsInitialized.Unregister(OnAdsInitialized);
    }

    public void OnAdsInitialized()
    {
        AdHandler.ShowAppOpen();
        StartCoroutine(LoadSceneAsync());
    }
    
    
    
    IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(splashScreenDuration);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName.MainMenu.ToString());
        
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            progressBar.fillAmount = progress;
            yield return null;
        }
    }
}

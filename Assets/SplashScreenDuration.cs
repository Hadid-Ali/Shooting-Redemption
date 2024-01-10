using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SplashScreenDuration : MonoBehaviour
{
    public float splashScreenDuration = 3f;
    public Image progressBar;

    [SerializeField] private GameObject consentPanel;

    [SerializeField] private GameObject adManagersAndroid;
    [SerializeField] private GameObject adManagersIOS;
    
    void Start()
    {
        consentPanel.SetActive(!Dependencies.GameDataOperations.GetConsent());
        StartCoroutine(LoadSceneAsync());
        GameEvents.GamePlayEvents.mainMenuButtonTap.Register(ButtonsOnClickExecution);


#if UNITY_ANDROID
        adManagersAndroid.SetActive(true);
#elif UNITY_IOS
        adManagersIOS.SetActive(true);
#else
        adManagersAndroid.SetActive(true);
#endif
        
        
    }

    private void OnDestroy()
    {
        GameEvents.GamePlayEvents.mainMenuButtonTap.UnRegister(ButtonsOnClickExecution);
    }

    private void ButtonsOnClickExecution(ButtonType type)
    {
        switch (type)    
        {
            case ButtonType.PrivacyPolicy:
                Application.OpenURL("https://play.virtua.com/privacy-policy");
                break;
            case ButtonType.AcceptConsent:
                Dependencies.GameDataOperations.SetConsent(true);
                consentPanel.SetActive(false);
                break;
        }
        print("Working");
    }
    
    IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(.5f);
        AdHandler.InitializeAds();
        GameAdEvents.InitFirebaseAnalytics.Raise();
        yield return new WaitForSeconds(splashScreenDuration);
        
        AdHandler.ShowAppOpen();
        FirebaseEvents.logEvent("Game Started");
        
        
        while (!Dependencies.GameDataOperations.GetConsent())
        {
            yield return null;
        }
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName.MainMenu.ToString());
        
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            progressBar.fillAmount = progress;
            yield return null;
        }
    }
}

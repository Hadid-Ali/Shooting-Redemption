using System;
using Firebase;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.Events;

public class FirebaseAnalytics : MonoBehaviour
{
    //public UnityEvent OnInitialized = new UnityEvent();
    private InitializationFailedEvent OnInitializationFailed = new InitializationFailedEvent();

    private void OnEnable()
    {
        GameAdEvents.InitFirebaseAnalytics.Register(InitFB);
    }

    void Start()
    {
        GameAdEvents.InitFirebaseAnalytics.Unregister(InitFB);
    }

    void InitFB()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                OnInitializationFailed.Invoke(task.Exception);
            }
            else
            {
                //OnInitialized.Invoke();
                Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                Debug.Log("Firebase Initialised");
            }
        });
    }

    [Serializable]
    public class InitializationFailedEvent : UnityEvent<Exception>
    {
        
    }
}

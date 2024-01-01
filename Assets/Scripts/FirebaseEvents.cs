using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FirebaseEvents
{
    public static void logEvent(string Msg)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent(Msg);
        
        Debug.Log(Msg);
    }
}

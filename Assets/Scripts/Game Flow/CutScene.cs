using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    public static GameEvent CutSceneEnded = new();
    
    private void OnEnable()
    {
        StartCoroutine(wait());
    }
    

    IEnumerator wait()
    {
        yield return new WaitForSeconds(.1f);
        CutSceneEnded.Raise();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum LevlsTypeSTatus
{
    todo,
    Doing,
    Done,
    Boss
}

public class LevelDataController : MonoBehaviour
{
    /*private Text statusText;*/
    public Image lvlImage;
    
    public void SetStatus(LevlsTypeSTatus sTatus,int levelNum)
    {
        
        /*statusText.text = "Level " + levelNum;*/
        
        
        switch (sTatus)
        {
            case LevlsTypeSTatus.Doing:
                lvlImage.color = Color.yellow;
                break;

            case LevlsTypeSTatus.Done:
                lvlImage.color = Color.green;
                break;

            case LevlsTypeSTatus.todo:
                lvlImage.color = Color.white;
                break;

            case LevlsTypeSTatus.Boss:
                lvlImage.color = Color.red;
                break;
        }
    }
}

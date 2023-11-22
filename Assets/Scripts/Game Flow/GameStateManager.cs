using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameObject LevelsButtonsParent;
    private List<Button> LevelsButtons;
    

    

    public void OnLevelSelect(int level)
    {
        SceneManager.LoadScene(level);
    }


}

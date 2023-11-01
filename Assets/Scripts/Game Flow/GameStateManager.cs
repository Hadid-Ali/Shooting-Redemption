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
    private GameObject PauseScreen;
    [SerializeField] private TextMeshProUGUI title;


    [SerializeField] private GameObject LevelsButtonsParent;
    private List<Button> LevelsButtons;
    
    private void Start()
    {
        title.SetText("Select level to Start Game");


        PauseScreen = transform.GetChild(0).gameObject;
        LevelsButtons = new ();
        for (int i = 0; i < LevelsButtonsParent.transform.childCount; i++)
        {
            int index = i + 1;
            LevelsButtons.Add(LevelsButtonsParent.transform.GetChild(i).GetComponent<Button>());
            LevelsButtonsParent.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(()=>OnLevelSelect(index));
        }

    }
    

    public void OnLevelSelect(int level)
    {
        SceneManager.LoadScene(level);
    }


}

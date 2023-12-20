using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GamePlayTutorial : MonoBehaviour
{
    [FormerlySerializedAs("_aimTapButton")] [SerializeField] private Button aimTapButton; 
    [FormerlySerializedAs("_aimingAnimation")] [SerializeField] private GameObject aimingAnimation;

    private TextMeshProUGUI tutorialText;

    private bool _showTutorial = true;
    
    private bool _buttonPressed;

    private void Awake()
    {
        aimingAnimation.SetActive(false);
        aimTapButton.gameObject.SetActive(false);

        tutorialText = GetComponentInChildren<TextMeshProUGUI>();
        
        if(_showTutorial)
            GameEvents.GamePlayEvents.OnCutSceneFinished.Register(OnButtonDown);

        
        GameEvents.GamePlayEvents.OnTutorialFinished.Register(OnTutorialFinished);
    }

    private void OnDestroy()
    {
        GameEvents.GamePlayEvents.OnTutorialFinished.Unregister(OnTutorialFinished);
        
        if(_showTutorial)
            GameEvents.GamePlayEvents.OnCutSceneFinished.Unregister(OnButtonDown);
    }
    
    

    public void OnButtonDown()
    {
        aimingAnimation.SetActive(true); 
        tutorialText.SetText("Swipe to Aim");
    }

    public void OnButtonUp()
    {
        aimTapButton.gameObject.SetActive(true);
        tutorialText.SetText("Tap on the Button");
    }
    
    private void OnTutorialFinished()
    {
        aimingAnimation.SetActive(false);
        aimTapButton.gameObject.SetActive(false);
        
        tutorialText.gameObject.SetActive(false);
    }
    
}

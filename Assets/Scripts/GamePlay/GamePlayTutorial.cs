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
    [SerializeField] private GameObject aimTapButton; 
    [SerializeField] private GameObject aimingAnimation;

    private TextMeshProUGUI tutorialText;

    private bool _showTutorial = true;
    
    private bool _buttonPressed;

    private void Awake()
    {
        GameEvents.GamePlayEvents.OnTutorialFinished.Register(OnTutorialFinished);
        GameEvents.GamePlayEvents.OnCutSceneFinished.Register(OnCutSceneCompleted);
    }

    private void OnCutSceneCompleted()
    {
        tutorialText = GetComponentInChildren<TextMeshProUGUI>();

        if (Dependencies.GameDataOperations.GetSelectedEpisode() == 0 && Dependencies.GameDataOperations.GetSelectedLevel() == 0)
        {
            PlayerInputt.OnZoom += OnButtonDown;
            PlayerInputt.OnUnZoom += OnButtonUp;
            aimTapButton.gameObject.SetActive(true);
        }
        
    }

    private void OnDestroy()
    {
        GameEvents.GamePlayEvents.OnTutorialFinished.Unregister(OnTutorialFinished);
        GameEvents.GamePlayEvents.OnCutSceneFinished.Unregister(OnCutSceneCompleted);

        if (_showTutorial)
        {
            PlayerInputt.OnZoom -= OnButtonDown;
            PlayerInputt.OnUnZoom -= OnButtonUp;
        }
    }
    
    

    public void OnButtonDown()
    {
        aimTapButton.gameObject.SetActive(false);
        aimingAnimation.SetActive(true); 
        tutorialText.SetText("Swipe to Aim");
    }

    public void OnButtonUp()
    {
        aimTapButton.gameObject.SetActive(true);
        aimingAnimation.SetActive(false); 
        tutorialText.SetText("Tap on the Button");
    }
    
    private void OnTutorialFinished()
    {
        aimingAnimation.SetActive(false);
        aimTapButton.gameObject.SetActive(false);

        StartCoroutine(Wait());
        
        if (_showTutorial)
        {
            PlayerInputt.OnZoom -= OnButtonDown;
            PlayerInputt.OnUnZoom -= OnButtonUp;
        }
    }

    IEnumerator Wait()
    {
        if(tutorialText != null)
            tutorialText.SetText("GOOD JOB !");
        
        yield return new WaitForSeconds(2f);
        
        if(tutorialText != null)
            tutorialText.gameObject.SetActive(false);
    }
    
}

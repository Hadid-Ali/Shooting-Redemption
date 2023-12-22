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

    private bool _isVeryFirstlevel;
    
    private bool _buttonPressed;

    private void Awake()
    {
        GameEvents.GamePlayEvents.OnTutorialFinished.Register(OnTutorialFinished);
        GameEvents.GamePlayEvents.OnCutSceneFinished.Register(OnCutSceneCompleted);
    }

    private void OnCutSceneCompleted()
    {
        _isVeryFirstlevel = Dependencies.GameDataOperations.GetSelectedEpisode() == 0 &&
                        Dependencies.GameDataOperations.GetSelectedLevel() == 0;
        tutorialText = GetComponentInChildren<TextMeshProUGUI>();
        
        if (_isVeryFirstlevel && !Dependencies.GameDataOperations.GetTutorialShown())
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

        if (_isVeryFirstlevel && !Dependencies.GameDataOperations.GetTutorialShown())
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
        if (_isVeryFirstlevel && !Dependencies.GameDataOperations.GetTutorialShown())
        {
            aimingAnimation.SetActive(false);
            aimTapButton.gameObject.SetActive(false);
            
            PlayerInputt.OnZoom -= OnButtonDown;
            PlayerInputt.OnUnZoom -= OnButtonUp;

            StartCoroutine(Wait());
            Dependencies.GameDataOperations.SetTutorialShown(true);
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

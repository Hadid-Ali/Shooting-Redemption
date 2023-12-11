using System;
using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    [SerializeField] private GameObject[] cams;


    private void Awake()
    {
        GameEvents.GamePlayEvents.ShowingBoss.Register(EnableCams);
        GetComponent<CharacterHealth>().Died += CanccelEvent;
    }

    private void CanccelEvent()
    {
        GameEvents.GamePlayEvents.ShowingBoss.UnRegister(EnableCams);
    }

    private void OnDestroy()
    {
        GameEvents.GamePlayEvents.ShowingBoss.UnRegister(EnableCams);
        GetComponent<CharacterHealth>().Died -= CanccelEvent;
    }

    private void EnableCams(bool val)
    {
        foreach (var v in cams)
            v.SetActive(val);
    }
}

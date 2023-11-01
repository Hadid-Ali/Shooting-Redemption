using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    [SerializeField] private GameObject[] cams;


    private void Awake()
    {
        EnemyGroupEvents.ShowBoss.Register(EnableCams);
    }

    private void OnDestroy()
    {
        EnemyGroupEvents.ShowBoss.UnRegister(EnableCams);
    }

    private void EnableCams(bool val)
    {
        foreach (var v in cams)
            v.SetActive(val);
    }
}

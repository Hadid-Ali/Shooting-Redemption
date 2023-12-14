using System;
using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelDifficulty : MonoBehaviour
{
    private CharacterHealth _characterHealth;


    [SerializeField] private float difficultyFactor = .1f;
    private void Start()
    {
        _characterHealth = GetComponent<CharacterHealth>();

        int Episode = Dependencies.GameDataOperations.GetSelectedEpisode();
        int level = Dependencies.GameDataOperations.GetSelectedLevel();

        float damageMultiplierCalculated = 1 + ((Episode * level) * difficultyFactor);

        _characterHealth.DamageMultiplier = damageMultiplierCalculated;    
    }
}

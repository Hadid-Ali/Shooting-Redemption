using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using UnityEngine;

public partial class GameEvents 
{
    public static partial class GamePlayEvents
    {
        //Player
        public static GameEvent<Transform> OnRequestPlayerPosition = new();
        public static GameEvent OnPlayerSpawned = new();
        public static GameEvent OnPlayerReachedCover = new();
        
        //Enemies
        public static GameEvent<GameplayStats> OnEnemyKilled = new ();
        public static GameEvent<CharacterHealth> OnEnemyResurrected = new ();
        
        //Group
        public static GameEvent<bool> ShowingBoss = new();
        
        public static GameEvent OnAllGroupsCleared = new();
        public static GameEvent<Transform> OnEnemyGroupKilled = new();



    }
}

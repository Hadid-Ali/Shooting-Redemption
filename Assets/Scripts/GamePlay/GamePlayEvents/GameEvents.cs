using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using UnityEngine;

public partial class GameEvents 
{
    public static partial class GamePlayEvents
    {
        //Player
        public static GameEvent OnPlayerSpawned = new();
        public static GameEvent OnPlayerReachedCover = new();
        public static GameEvent OnPlayerHit = new();
        public static GameEvent OnPlayerDead = new();
        public static GameEvent OnCutSceneFinished = new();

        public static GameEvent OnTutorialFinished = new();
        
        //Enemies
        public static GameEvent<GameplayStats> OnEnemyKilled = new ();
        public static GameEvent<CharacterHealth> OnEnemyResurrected = new ();
        
        //Group
        public static GameEvent<bool> ShowingBoss = new();
        
        public static GameEvent OnAllGroupsCleared = new();
        public static GameEvent<Transform> OnEnemyGroupKilled = new();
        
        //GameLost
        public static GameEvent TimeOver = new();
        public static GameEvent OnNPCKilled = new();
        
        //UI Events
        public static GameEvent<int> OnUpdateCoins = new();
        public static GameEvent<ButtonType> mainMenuButtonTap = new();
        public static GameEvent<ButtonType, bool> updateButton = new();

        public static GameEvent OnInterstitialClosed = new();

        public static GameEvent OnLevelPause = new();
        public static GameEvent OnLevelResumed = new();
        



    }
}

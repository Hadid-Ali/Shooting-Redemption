using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "GameData/GameData/Create", order = 1)]
public class GameData : ScriptableObject
{

 [Header("Setting")] 
 public bool sound;
 public bool haptic;
 public bool shadow;
 
    public string DateTime;
    public int m_Coins = 100000;
    public int m_Stars;
    public int m_SelectedLevel;
    public int m_SelectedEpisode;
    public int m_SelectedGunIndex;
    public int[] m_UnlockedLevels = new int[5];
    public int m_UnlockedEpisodes ;
    public int m_SelectedCharacterIndex;
    public List<Characters> CharacterData;
    public List<Guns> GunsData;
    public float playerHealth;
    
    

    [Space]
    [Header("Upgrades")]
    
    
    public int currentHealthProgress;
   // public List<UpgradeProgress> health;
    
    public int currentCoinMagnetProgress;
  //  public List<UpgradeProgress> coinMagnet;
    
    public int currentGrenadeProgress;
   // public List<UpgradeProgress> Grenades;


    [Space] [Header("Kills Data")] 
    public int TotalHeadShot;
    public int TotalKilled;
    public int BankerKilled;
    [Space]
    [Header("Daily Goals")] 
    
    public int DailyGoalComplete;
    
    public GoalProgress kills;
    
    public GoalProgress Headshot;
    
    public GoalProgress banker;
    
    
    [Space]
    [Header("Achievements")] 
    
   // public int AchievementGoalComplete;
    
    public List<GoalProgress> Achievementkills;
    
    public List<GoalProgress> AchievementHeadshot;
    
    public List<GoalProgress> Achievementbanker;

    

    /*public void InitializeCompletedLevels()
    {
     completedLevelsPerEpisode = new bool[5, 5];
    }

    public bool IsLevelCompleted(int episode, int level)
    {
     return completedLevelsPerEpisode[episode, level];
    }

    public void CompleteLevel(int episode, int level,bool val)
    {
     completedLevelsPerEpisode[episode, level] = val;
    }*/
    
   
}

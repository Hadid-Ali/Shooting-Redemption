using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GoalProgress
{
    public bool taskComplete;
    public int DailyGoalStars;
    public int DailyGoalCoin;//index
    public int DailyGoalValue;
    public int CurrentAchieveValue;
    
    
    public bool onDailyGoalAchieve()
    {
        if (CurrentAchieveValue >= DailyGoalValue)
        {
            return true;
        }

        return false;
    }
    
    public bool onAchievemnentGoalAchieve(int currentProgress)
    {
        if (currentProgress >= DailyGoalValue)
        {
            return true;
        }

        return false;
    }

    public void reset()
    {
        taskComplete = false;
        CurrentAchieveValue = 0;
    }
}

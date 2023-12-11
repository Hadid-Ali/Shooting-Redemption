using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DailyGoalHandler : MonoBehaviour
{
//     
//     [SerializeField] private DailyTask RedmanKillTask; 
//     [SerializeField] private DailyTask HeadShotTask; 
//     [SerializeField] private DailyTask BankerTask; 
//     
//     private int totalCoins;
//     private int totalStars;
//     private void Start()
//     {
//       //  CheckTImeDate();
//         totalCoins = Dependencies.GameDataOperations.GetCoins();
//         //totalStars = SaveLoadData.GameData.m_Stars;
//         updateDailyGoalData();
//     }
//     
//     public void updateDailyGoalData()
//     {
//         /*RedmanKillTask.UpdateTaskData(SaveLoadData.GameData.kills.DailyGoalValue,SaveLoadData.GameData.kills.CurrentAchieveValue);
//         HeadShotTask.UpdateTaskData(SaveLoadData.GameData.Headshot.DailyGoalValue,SaveLoadData.GameData.Headshot.CurrentAchieveValue);
//         BankerTask.UpdateTaskData(SaveLoadData.GameData.banker.DailyGoalValue,SaveLoadData.GameData.banker.CurrentAchieveValue);
//         CheckGoals();*/
//     }
//     
//     /*private void CheckGoals()
//     {
//         if(SaveLoadData.GameData.kills.onDailyGoalAchieve())
//         {
//             SaveLoadData.GameData.kills.taskComplete = true;
//           //  RedmanKillTask.TaskComplete();
//             Debug.Log("You completed 30 kills goal! Earned 50 coins.");
//         }
//
//         if (SaveLoadData.GameData.Headshot.onDailyGoalAchieve())
//         {
//             SaveLoadData.GameData.Headshot.taskComplete = true;
//            // HeadShotTask.TaskComplete();
//             Debug.Log("You completed 10 headshots goal! Earned 50 coins.");
//         }
//
//         if (SaveLoadData.GameData.banker.onDailyGoalAchieve())
//         {
//             SaveLoadData.GameData.banker.taskComplete = true;
//            //BankerTask.TaskComplete();
//             Debug.Log("You completed 30 banker kills goal! Earned 50 coins.");
//         }
//         
//         
//     }*/
//
//
//     /*public void CheckTImeDate()
//     {
//         if (TimeManager.Instance.GetTime() <= 0)
//         {
//             resetDailyReward();
//             SaveLoadData.GameData.DateTime = DateTime.Now.ToBinary().ToString();
//             SaveLoadData.SaveData();
//         }
//     }
//
//     public void resetDailyReward()
//     {
//         SaveLoadData.GameData.kills.reset();
//         SaveLoadData.GameData.banker.reset();
//         SaveLoadData.GameData.Headshot.reset();
//         SaveLoadData.SaveData();
//     }*/
}

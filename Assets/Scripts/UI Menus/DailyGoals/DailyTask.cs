using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyTask : MonoBehaviour
{
   // public Image completeCheck;
    public Text taskDataText;

    public void UpdateTaskData(int Max, int Current)
    {
        print("update task");
        taskDataText.text = $"Kills: {Current}/{Max}";
    }

    /*public void TaskComplete()
    {
        completeCheck.gameObject.SetActive(true);
    }*/
}

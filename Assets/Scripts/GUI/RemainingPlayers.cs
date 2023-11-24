using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;

public class RemainingPlayers : MonoBehaviour
{
    private TextMeshProUGUI text;

    private int remainingEnemies;
    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();

        AIGroupsHandler.OnEnemyKilledUIUpdate += UpdateText;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        remainingEnemies = AiGroup.TotalEnemiesCount + 1;
        UpdateText(0);
    }

    private void OnDestroy()
    {
        AIGroupsHandler.OnEnemyKilledUIUpdate -= UpdateText;
    }

    public void UpdateText(int totalPlayers)
    {
        remainingEnemies--;
        
        if (totalPlayers > 0)
            text.SetText(remainingEnemies + "/" + AiGroup.TotalEnemiesCount * totalPlayers);
        else
            text.SetText(remainingEnemies + "/" + AiGroup.TotalEnemiesCount);
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsPanel : UIMenuBase
{
    public List<Button> buttons;
    
    protected override void OnMenuContainerEnable()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int j = i;
            buttons[j].interactable = j < Dependencies.GameDataOperations.GetUnlockedLevels(Dependencies.GameDataOperations.GetSelectedEpisode());
            buttons[j].onClick.AddListener(()=> SetLevelNum(j));
        }
        
    }

    protected override void OnMenuContainerDisable()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int j = i;
            buttons[j].onClick.RemoveAllListeners();
        }
    }

    public void SetLevelNum(int i)
    {
        StartCoroutine(LoadAsyncScene("Chapter" + (Dependencies.GameDataOperations.GetSelectedEpisode() + 1)));
    }

    IEnumerator LoadAsyncScene(string m_LoadScene)
    {
        print(m_LoadScene + "m_LoadScene");
        yield return new WaitForSeconds(.5f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_LoadScene);
        yield return new WaitForSeconds(.5f);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    public void OnCloseButtonClicked()
    {
        ChangeMenuState(MenuName.EpisodeSelection);
    }
}

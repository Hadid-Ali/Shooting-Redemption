using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static int Episode; 
    public static int Level;
    
    public Transform LevelPanel;
    
    public void SetLevelNum()
    {
        Debug.LogError("Episode" + (Episode+1) + " Level " + (Level+1));
        StartCoroutine(LoadAsyncScene("Chapter" + (Episode+1)));
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

    void BacktoMainMenu()
    {
        /*SoundHandler.Instence.playSound(SoundHandler.Instence.m_Back);*/
        SceneManager.LoadScene("MainMenu");
    }

}

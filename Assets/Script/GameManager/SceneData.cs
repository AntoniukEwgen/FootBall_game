using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class SceneData
{
    public Dictionary<string, bool> gameData = new Dictionary<string, bool>();

    public SceneData()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            gameData[sceneName] = false;
        }
    }

    public void UpdateSceneData(string sceneName, bool isWinner)
    {
        gameData[sceneName] = isWinner;
    }

    public void SaveSceneData()
    {
        foreach (KeyValuePair<string, bool> entry in gameData)
        {
            PlayerPrefs.SetInt(entry.Key, entry.Value ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    public void LoadSceneData()
    {
        foreach (KeyValuePair<string, bool> entry in gameData.ToList())
        {
            if (PlayerPrefs.HasKey(entry.Key))
            {
                gameData[entry.Key] = PlayerPrefs.GetInt(entry.Key) == 1;
            }
        }
    }

    public void PrintSceneData()
    {
        foreach (KeyValuePair<string, bool> entry in gameData)
        {
            Debug.Log("Scene: " + entry.Key + ", WinnerGame: " + entry.Value);
        }
    }
}
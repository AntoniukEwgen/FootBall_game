using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    public void FinishLvl()
    {
        PlayerPrefs.SetInt("currentScene", SceneManager.GetActiveScene().buildIndex);
    }
}

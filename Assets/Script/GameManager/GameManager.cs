using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private BallCoins ballCoin;
    public GameObject tutorial;

    [Header("Coin Counter")]
    [SerializeField] private TMP_Text winText;
    [SerializeField] FinishLevel finishLevel;

    public bool isPaused;
    private SceneData sceneData;

    private void Start()
    {
        AudioManager.Instance.AddButtonSound();
        InitializePanels();
        sceneData = new SceneData();
        sceneData.LoadSceneData();
        sceneData.PrintSceneData();
    }

    private void InitializePanels()
    {
        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        pausePanel.SetActive(isPaused);
    }

    public void GoToMainMenu() => LoadScene("MainMenu");

    public void ReloadGame() => LoadScene(SceneManager.GetActiveScene().name);

    public void LoadNextScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

    public void LoseGame()
    {
        PauseGame();
        losePanel.SetActive(true);
        ResetCoinCount();
    }

    public void WinGame()
    {
        PauseGame();
        winPanel.SetActive(true);
        UpdateSceneData();
        SaveCoins();
        DisplayWinText();
        finishLevel.FinishLvl();
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    private void ResetCoinCount()
    {
        BallCoins coin = ballCoin;
        if (coin != null)
        {
            coin.ResetCoinCount();
        }
        else
        {
            Debug.LogError("Player object not found");
        }
    }

    private void UpdateSceneData()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        sceneData.UpdateSceneData(currentScene, true);
        sceneData.SaveSceneData();
        sceneData.PrintSceneData();
    }

    private void SaveCoins()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        BallCoins coin = ballCoin;
        if (coin != null)
        {
            totalCoins += coin.CoinCount;
        }
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
    }

    private void DisplayWinText()
    {
        BallCoins coin = ballCoin;
        if (coin != null)
        {
            winText.text = coin.CoinCount.ToString();
        }
        else
        {
            Debug.LogError("Player object not found");
        }
    }

    private void LoadScene(string sceneName)
    {
        isPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
}

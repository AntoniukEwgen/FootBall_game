using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Кнопки")]
    [SerializeField] private Button openLevelsButton;
    [SerializeField] private Button closeLevelsButton;
    [Header("----------------------------------")]
    [SerializeField] private Button openShopingButton;
    [SerializeField] private Button closeShopingButton;
    [Header("----------------------------------")]
    [SerializeField] private Button openSettingsButton;
    [SerializeField] private Button closeSettingsButton;
    [Header("----------------------------------")]
    [SerializeField] private Button quitButton;

    [Header("Панелі гри")]
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Відображення монет")]
    [SerializeField] private TMP_Text[] coinText;

    public static int currLevel;

    void Start()
    {
        AudioManager.Instance.AddButtonSound();

        PanelActive();
        LoadCoins();

        openLevelsButton.onClick.AddListener(() => levelPanel.SetActive(true));
        closeLevelsButton.onClick.AddListener(() => levelPanel.SetActive(false));

        openShopingButton.onClick.AddListener(() => shopPanel.SetActive(true));
        closeShopingButton.onClick.AddListener(() => shopPanel.SetActive(false));

        openSettingsButton.onClick.AddListener(() => settingsPanel.SetActive(true));
        closeSettingsButton.onClick.AddListener(() => settingsPanel.SetActive(false));

        quitButton.onClick.AddListener(Application.Quit);
    }

    private void PanelActive()
    {
        levelPanel.SetActive(false);
        shopPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void OnClickLevel(int levelNum)
    {
        currLevel = levelNum;
        SceneManager.LoadScene("Level_" + levelNum);
    }

    public void LoadCoins()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        foreach (TMP_Text text in coinText) 
        {
            text.text = "" + totalCoins;
        }
    }
}

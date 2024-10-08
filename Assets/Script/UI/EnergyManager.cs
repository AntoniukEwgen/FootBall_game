using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public int maxEnergy = 20;
    public float restoreDuration = 5f;
    public TMP_Text energyText;
    public Button energyButton;

    private int currentEnergy;
    private float restoreTimer;

    private void Start()
    {
        currentEnergy = PlayerPrefs.GetInt("Energy", maxEnergy);

        var lastTime = PlayerPrefs.GetString("LastSession", "");
        if (!string.IsNullOrEmpty(lastTime))
        {
            var lastSession = System.DateTime.Parse(lastTime);
            var difference = System.DateTime.Now - lastSession;
            var energyToRestore = Mathf.FloorToInt((float)difference.TotalSeconds / restoreDuration);
            currentEnergy = Mathf.Min(currentEnergy + energyToRestore, maxEnergy);
        }

        UpdateEnergyText();
    }

    private void Update()
    {
        if (currentEnergy < maxEnergy)
        {
            restoreTimer += Time.deltaTime;
            if (restoreTimer >= restoreDuration)
            {
                currentEnergy++;
                UpdateEnergyText();
                restoreTimer = 0f;
            }
        }
    }

    public void UseEnergy()
    {
        if (currentEnergy > 0)
        {
            currentEnergy--;
            UpdateEnergyText();
        }
    }

    public void AddEnergy(int amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        UpdateEnergyText();
    }

    public int CurrentEnergy
    {
        get { return currentEnergy; }
    }

    public int MaxEnergy
    {
        get { return maxEnergy; }
    }

    private void UpdateEnergyText()
    {
        energyText.text = $"{currentEnergy}/{maxEnergy}";
        energyButton.interactable = currentEnergy > 0;

        PlayerPrefs.SetInt("Energy", currentEnergy);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LastSession", System.DateTime.Now.ToString());
    }
}

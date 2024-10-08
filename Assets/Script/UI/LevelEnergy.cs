using UnityEngine;
using UnityEngine.UI;

public class LevelEnergy : MonoBehaviour
{
    public Button[] energyButtons;
    public int maxEnergy = 20; 
    private int currentEnergy; 

    private void Start()
    {
        currentEnergy = PlayerPrefs.GetInt("Energy", maxEnergy);

        foreach (var button in energyButtons)
        {
            button.onClick.AddListener(() =>
            {
                if (currentEnergy > 0)
                {
                    currentEnergy--;
                    PlayerPrefs.SetInt("Energy", currentEnergy);
                }
                UpdateButtonInteractivity();
            });
        }
        UpdateButtonInteractivity();

    }

    private void UpdateButtonInteractivity()
    {
        foreach (var button in energyButtons)
        {
            button.interactable = currentEnergy > 0;
        }
    }
}

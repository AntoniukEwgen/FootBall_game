using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyShop : MonoBehaviour
{
    [SerializeField] private EnergyManager energyManager;
    [SerializeField] private MainMenuManager mainMenuManager;
    [SerializeField] private Button buyEnergyButton;
    [SerializeField] private int energyCost = 10;
    [SerializeField] private int energyAmount = 1;

    private void Start()
    {
        buyEnergyButton.onClick.AddListener(BuyEnergy);
    }

    private void Update()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        buyEnergyButton.interactable = totalCoins >= energyCost;
    }

    public void BuyEnergy()
    {
        if (energyManager.CurrentEnergy >= energyManager.MaxEnergy)
        {
            Debug.Log("У вас вже є максимальна кількість енергії");
            return;
        }

        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        int totalCost = energyCost * energyAmount;

        if (totalCoins >= totalCost)
        {
            totalCoins -= totalCost;
            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            mainMenuManager.LoadCoins();
            energyManager.AddEnergy(energyAmount);
        }
        else
        {
            Debug.Log("Недостатньо монет для покупки енергії");
        }
    }
}

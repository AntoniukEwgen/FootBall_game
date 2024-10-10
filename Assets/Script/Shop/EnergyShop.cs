using UnityEngine;
using UnityEngine.UI;

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
        buyEnergyButton.interactable = PlayerPrefs.GetInt("TotalCoins", 0) >= energyCost;
    }

    public void BuyEnergy()
    {
        if (energyManager.CurrentEnergy >= energyManager.MaxEnergy)
        {
            Debug.Log("You already have maximum energy levels!");
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
            Debug.Log("Not enough coins to make a purchase!");
        }
    }
}
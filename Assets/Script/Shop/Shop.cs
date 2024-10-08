using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [System.Serializable]
    public struct Item
    {
        public string name;
        public int price;
        public Button button;
        public GameObject noPurshase;
        public GameObject purshase;

        public GameObject bought;
        public GameObject selected;
    }

    public Item[] items;
    public MainMenuManager mainMenuManager;

    private void Start()
    {
        LoadData();

        foreach (var item in items)
        {
            item.button.onClick.AddListener(() => HandleItem(item));
        }

        if (PlayerPrefs.GetString("SelectedItem", "") == "")
        {
            var skin1 = System.Array.Find(items, item => item.name == "Skin_1");
            if (skin1.name != null)
            {
                PlayerPrefs.SetInt(skin1.name, 1);
                PlayerPrefs.SetString("SelectedItem", skin1.name);
                LoadData();
            }
        }
    }
    private void LoadData()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        foreach (var item in items)
        {
            if (PlayerPrefs.GetInt(item.name, 0) == 1)
            {
                item.noPurshase.SetActive(false);
                item.purshase.SetActive(true);
                item.bought.SetActive(true);
                item.selected.SetActive(false);
            }
            else
            {
                item.button.interactable = totalCoins >= item.price;
            }

            if (PlayerPrefs.GetString("SelectedItem", "") == item.name)
            {
                item.bought.SetActive(false);
                item.selected.SetActive(true);
            }
        }
    }

    private void HandleItem(Item item)
    {
        if (PlayerPrefs.GetInt(item.name, 0) == 1)
        {
            SelectItem(item);
        }
        else
        {
            BuyItem(item);
        }
    }

    private void BuyItem(Item item)
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        if (totalCoins >= item.price)
        {
            totalCoins -= item.price;
            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            mainMenuManager.LoadCoins();

            item.noPurshase.SetActive(false);
            item.purshase.SetActive(true);
            item.bought.SetActive(true);
            item.selected.SetActive(false);

            PlayerPrefs.SetInt(item.name, 1);

            SelectItem(item);
        }
        else
        {
            Debug.Log("Недостатньо монет для покупки " + item.name);
        }

        LoadData();
    }

    public void SelectItem(Item item)
    {
        foreach (var i in items)
        {
            if (PlayerPrefs.GetInt(i.name, 0) == 1)
            {
                i.bought.SetActive(true);
                i.selected.SetActive(false);
            }
        }

        if (PlayerPrefs.GetInt(item.name, 0) == 1)
        {
            item.bought.SetActive(false);
            item.selected.SetActive(true);
            PlayerPrefs.SetString("SelectedItem", item.name);
        }
    }
}

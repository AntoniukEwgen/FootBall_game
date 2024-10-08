using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCoins : MonoBehaviour
{
    public int CoinCount { get; private set; } = 0;

    private bool coinTriggered = false; 
    public delegate void CoinTriggered();
    public static event CoinTriggered OnCoinTriggered;
    [SerializeField] private AudioClip _scoreClip;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coins"))
        {
            AudioManager.Instance.PlaySound(_scoreClip);
            CoinCount++;
            coinTriggered = true; 
            OnCoinTriggered?.Invoke(); 
        }
    }

    public bool IsCoinTriggered()
    {
        return coinTriggered;
    }

    public void ResetCoinTriggered()
    {
        coinTriggered = false;
    }
    public void ResetCoinCount()
    {
        CoinCount = 0;
    }
}

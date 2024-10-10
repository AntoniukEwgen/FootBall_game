using UnityEngine;

public class CoinCollectorHandler : MonoBehaviour
{
    [SerializeField] private AudioClip _coinCollectClip;

    public int Balance { get; private set; } = 0;
    public delegate void CoinCollected();
    public static event CoinCollected OnCoinCollected;

    private bool _isCoinCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Coins")) return;

        SoundManager.Instance.PlaySound(_coinCollectClip);
        Balance++;
        _isCoinCollected = true;
        OnCoinCollected?.Invoke();
    }

    public bool IsCoinCollected() =>
        _isCoinCollected;

    public void ResetCoinCollectedStatus() =>
        _isCoinCollected = false;

    public void ResetBalance() =>
        Balance = 0;
}
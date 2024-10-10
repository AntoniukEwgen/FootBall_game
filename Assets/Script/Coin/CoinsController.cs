using UnityEngine;

public class CoinsController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball")) return;

        Destroy(gameObject);
    }
}

using UnityEngine;

public class CoinsController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Destroy(gameObject);
        }
    }
}

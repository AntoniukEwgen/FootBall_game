using System.Collections;
using UnityEngine;
using TMPro;
using System.Threading;

public class Target : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 2.0f;
    [SerializeField] private float restoreDuration = 2.0f;
    [SerializeField] private AudioClip winner;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private TextMeshProUGUI Timer;
    [SerializeField] private TextMeshProUGUI Value;
    [SerializeField] private TextMeshProUGUI YouValue;

    [SerializeField] private float timerValue = 120.0f;
    [SerializeField] private int valueCount = 10;
    private int hitCount = 0;


    public delegate void BallHitTarget();
    public static event BallHitTarget OnBallHitTarget;
    private bool ballTriggered = false;


    private void Start()
    {
        Timer.text = FormatTime(timerValue);
        Value.text = FormatCount(valueCount);
        YouValue.text = FormatCount(hitCount);
        StartCoroutine(StartTimer());
    }

    public bool IsBallTriggered()
    {
        return ballTriggered;
    }

    public void ResetBallTriggered()
    {
        ballTriggered = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball") && !ballTriggered)
        {
            ballTriggered = true;
            AudioManager.Instance.PlaySound(winner);
            Debug.Log("ћ'€ч потрапив у ц≥ль!");
            Interlocked.Increment(ref hitCount);
            YouValue.text = FormatCount(hitCount);
            OnBallHitTarget?.Invoke();
            if (hitCount >= valueCount)
            {
                gameManager.WinGame();
            }
            else
            {
                circleCollider.enabled = false; 
                StartCoroutine(FadeOutAndRestore());
            }
        }
    }

    IEnumerator FadeOutAndRestore()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        for (float t = 0.0f; t < fadeDuration; t += Time.deltaTime)
        {
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(1, 0, t / fadeDuration);
            spriteRenderer.color = color;

            yield return null;
        }

        yield return new WaitForSeconds(restoreDuration);

        for (float t = 0.0f; t < fadeDuration; t += Time.deltaTime)
        {
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(0, 1, t / fadeDuration);
            spriteRenderer.color = color;

            yield return null;
        }
        circleCollider.enabled = true; 
    }

    IEnumerator StartTimer()
    {
        while (timerValue > 0)
        {
            yield return new WaitForSeconds(1.0f);
            timerValue--;
            Timer.text = FormatTime(timerValue);
        }
        Debug.Log("„ас вийшов, ви програли!");
        gameManager.LoseGame();
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private string FormatCount(int count)
    {
        return string.Format("{0:00}", count);
    }
}

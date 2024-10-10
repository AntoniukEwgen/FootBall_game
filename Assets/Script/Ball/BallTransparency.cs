using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class BallTransparency : MonoBehaviour
{
    [SerializeField] private BallController ballController;
    [SerializeField] private TextMeshProUGUI spawnText;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CoinCollectorHandler coinInteraction;
    [SerializeField] private Target target;
    [SerializeField] private AudioClip lose;

    [SerializeField] private float delayInSeconds = 2f;
    [SerializeField] private float transparencySpeed = 1f;

    [SerializeField] private int MaxSpins = 3;
    private int currentSpins = 0;

    private SpriteRenderer ballRenderer;

    private void Start()
    {
        spawnText.text = MaxSpins.ToString();
        BallController.OnBallStopped += HandleBallStopped;
        ballRenderer = ballController.GetComponent<SpriteRenderer>();
    }

    private void OnDestroy()
    {
        BallController.OnBallStopped -= HandleBallStopped;
        Target.OnBallHitTarget -= HandleBallHitTarget;
    }

    private void HandleBallStopped(Vector2 position)
    {
        StartCoroutine(FadeOutAndMove());
    }

    private void HandleBallHitTarget()
    {
        target.ResetBallTriggered();
    }

    IEnumerator FadeOutAndMove()
    {
        yield return new WaitForSeconds(delayInSeconds);

        ballRenderer.DOFade(0f, transparencySpeed).OnComplete(() =>
        {
            if (!coinInteraction.IsCoinCollected() && !target.IsBallTriggered())
            {
                currentSpins++;

                if (currentSpins < MaxSpins)
                {
                    ResetBallPositionAndScale();
                    ballRenderer.DOFade(1f, transparencySpeed);
                }
                else
                {
                    ballRenderer.DOFade(0f, 0f);
                }

                if (currentSpins >= MaxSpins)
                {
                    SoundManager.Instance.PlaySound(lose);
                    gameManager.LoseGame();
                }

                UpdateSpawnText();
            }
            else
            {
                ResetBallPositionAndScale();
                ballRenderer.DOFade(1f, transparencySpeed);

                coinInteraction.ResetCoinCollectedStatus();
                target.ResetBallTriggered();
            }
        });
    }

    private void ResetBallPositionAndScale()
    {
        Vector3 newPosition = new Vector3(0f, -2.3f, 0f);
        ballController.transform.position = newPosition;
        ballController.transform.localScale = new Vector3(0.26f, 0.26f, 0.26f);
    }

    private void UpdateSpawnText()
    {
        spawnText.text = (MaxSpins - currentSpins).ToString();
    }
}

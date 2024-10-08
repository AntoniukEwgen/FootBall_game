using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using TMPro;

public class BallTransparency : MonoBehaviour
{
    [SerializeField] private Ball ball;
    [SerializeField] private TextMeshProUGUI spawnText;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private BallCoins coinInteraction;
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
        Ball.OnBallStopped += HandleBallStopped;
        ballRenderer = ball.GetComponent<SpriteRenderer>();
    }

    private void OnDestroy()
    {
        Ball.OnBallStopped -= HandleBallStopped;
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
            if (!coinInteraction.IsCoinTriggered() && !target.IsBallTriggered())
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
                    AudioManager.Instance.PlaySound(lose);
                    gameManager.LoseGame();
                }

                UpdateSpawnText();
            }
            else
            {
                ResetBallPositionAndScale();
                ballRenderer.DOFade(1f, transparencySpeed);

                coinInteraction.ResetCoinTriggered();
                target.ResetBallTriggered();
            }
        });
    }

    private void ResetBallPositionAndScale()
    {
        Vector3 newPosition = new Vector3(0f, -2.3f, 0f);
        ball.transform.position = newPosition;
        ball.transform.localScale = new Vector3(0.26f, 0.26f, 0.26f);
    }

    private void UpdateSpawnText()
    {
        spawnText.text = (MaxSpins - currentSpins).ToString();
    }
}

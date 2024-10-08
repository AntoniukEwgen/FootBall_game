using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core.Easing;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform minPoint;
    [SerializeField] private Transform maxPoint;
    [SerializeField] private SpriteRenderer shadow; 
    [SerializeField] private float speed = 1.0f;
    [SerializeField] GameManager gameManager;
    [SerializeField] private AudioClip _moveClip;

    private Vector2 startSwipe;
    private Vector2 endSwipe;
    private bool isSwipeOnBall;
    private readonly float initialScale = 0.26f;
    private readonly float finalScale = 0.08f;
    private readonly float maxAlpha = 130f / 255f; 


    public static event Action<Vector2> OnBallStopped = delegate { };

    void Start()
    {
        transform.localScale = new Vector3(initialScale, initialScale, initialScale);
        shadow.color = new Color(shadow.color.r, shadow.color.g, shadow.color.b, maxAlpha);
    }
    void Update()
    {
        if (gameManager.isPaused || (gameManager.tutorial != null && gameManager.tutorial.activeSelf))
        {
            AudioManager.Instance.StopSound(_moveClip);
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            startSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            isSwipeOnBall = IsSwipeOnBall(startSwipe);
        }

        if (Input.GetMouseButtonUp(0) && isSwipeOnBall)
        {
            endSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            AudioManager.Instance.PlaySound(_moveClip);
            Swipe();
        }
    }

    bool IsSwipeOnBall(Vector2 swipeStart)
    {
        Vector3 ballPosition = Camera.main.WorldToViewportPoint(transform.position);
        float distance = Vector2.Distance(swipeStart, ballPosition);
        return distance <= 0.2f;
    }

    void Swipe()
    {
        Vector2 swipe = endSwipe - startSwipe;
        if (swipe.y < 0)
            return;

        float swipeAngle = Vector2.SignedAngle(Vector2.right, swipe);
        float swipeForce = swipe.magnitude;
        Vector2 targetPosition = Vector2.Lerp(minPoint.position, maxPoint.position, swipeForce);

        targetPosition.x += Mathf.Cos(swipeAngle * Mathf.Deg2Rad) * swipeForce;
        targetPosition.y += Mathf.Sin(swipeAngle * Mathf.Deg2Rad) * swipeForce;

        StartCoroutine(MoveToTarget(targetPosition));
    }

    IEnumerator MoveToTarget(Vector2 target, bool isReturning = false)
    {
        float duration = Vector2.Distance((Vector2)transform.position, target) / speed;
        transform.DOScale(finalScale, duration);

        shadow.DOFade(isReturning ? maxAlpha : 0, duration);

        while ((Vector2)transform.position != target)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (transform.position.y > maxPoint.position.y)
            {
                transform.position = new Vector2(transform.position.x, maxPoint.position.y);
                if (!isReturning)
                {
                    OnBallStopped(transform.position);
                }
                break;
            }
            yield return null;
        }

        if (!isReturning && (Vector2)transform.position == target && transform.position.y <= maxPoint.position.y && transform.position.y >= minPoint.position.y)
        {
            OnBallStopped(transform.position); 
        }

        if (transform.position.y > minPoint.position.y)
        {
            yield return new WaitForSeconds(0.1f);
            Vector2 minPointWithCurrentX = new(transform.position.x, minPoint.position.y);
            StartCoroutine(MoveToTarget(minPointWithCurrentX, true));
        }
    }

}

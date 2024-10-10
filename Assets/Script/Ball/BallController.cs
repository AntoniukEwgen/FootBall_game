using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private Transform _minBoundaryPoint;
    [SerializeField] private Transform _maxBoundaryPoint;
    [SerializeField] private SpriteRenderer _shadowRenderer;
    [SerializeField] private float _movementSpeed = 1.0f;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private AudioClip _movementSoundClip;

    private Vector2 _startSwipePosition;
    private Vector2 _endSwipePosition;
    private bool _isSwipeOnBall;
    private const float InitialBallScale = 0.26f;
    private const float TargetBallScale = 0.08f;
    private const float MaxShadowAlpha = 130f / 255f;

    public static event Action<Vector2> OnBallStopped = delegate { };

    private void Start()
    {
        transform.localScale = Vector3.one * InitialBallScale;
        _shadowRenderer.color = new Color(_shadowRenderer.color.r, _shadowRenderer.color.g, _shadowRenderer.color.b, MaxShadowAlpha);
    }

    private void Update()
    {
        if (_gameManager.isPaused || (_gameManager.tutorial != null && _gameManager.tutorial.activeSelf))
        {
            SoundManager.Instance.StopSound(_movementSoundClip);
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _startSwipePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            _isSwipeOnBall = IsSwipeOnBall(_startSwipePosition);
        }

        if (Input.GetMouseButtonUp(0) && _isSwipeOnBall)
        {
            _endSwipePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            SoundManager.Instance.PlaySound(_movementSoundClip);
            Swipe();
        }
    }

    private bool IsSwipeOnBall(Vector2 swipeStart)
    {
        Vector3 ballPosition = Camera.main.WorldToViewportPoint(transform.position);
        float distance = Vector2.Distance(swipeStart, ballPosition);
        return distance <= 0.2f;
    }

    private void Swipe()
    {
        Vector2 swipe = _endSwipePosition - _startSwipePosition;
        if (swipe.y < 0) return;

        float swipeAngle = Vector2.SignedAngle(Vector2.right, swipe);
        float swipeForce = swipe.magnitude;
        Vector2 targetPosition = Vector2.Lerp(_minBoundaryPoint.position, _maxBoundaryPoint.position, swipeForce);

        targetPosition.x += Mathf.Cos(swipeAngle * Mathf.Deg2Rad) * swipeForce;
        targetPosition.y += Mathf.Sin(swipeAngle * Mathf.Deg2Rad) * swipeForce;

        StartCoroutine(MoveToTarget(targetPosition));
    }

    private IEnumerator MoveToTarget(Vector2 target, bool isReturning = false)
    {
        float duration = Vector2.Distance(transform.position, target) / _movementSpeed;
        transform.DOScale(TargetBallScale, duration);

        _shadowRenderer.DOFade(isReturning ? MaxShadowAlpha : 0, duration);

        while ((Vector2)transform.position != target)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, _movementSpeed * Time.deltaTime);

            if (transform.position.y > _maxBoundaryPoint.position.y)
            {
                transform.position = new Vector2(transform.position.x, _maxBoundaryPoint.position.y);
                if (!isReturning)
                {
                    OnBallStopped?.Invoke(transform.position);
                }
                break;
            }
            yield return null;
        }

        if (!isReturning && (Vector2)transform.position == target &&
            transform.position.y <= _maxBoundaryPoint.position.y &&
            transform.position.y >= _minBoundaryPoint.position.y)
        {
            OnBallStopped?.Invoke(transform.position);
        }

        if (transform.position.y > _minBoundaryPoint.position.y)
        {
            yield return new WaitForSeconds(0.1f);
            Vector2 minPointWithCurrentX = new(transform.position.x, _minBoundaryPoint.position.y);
            StartCoroutine(MoveToTarget(minPointWithCurrentX, true));
        }
    }
}
using UnityEngine;
using System.Collections;
using System;

public class GoalAnimation : MonoBehaviour
{
    public Sprite sprite1;
    public Sprite sprite2;
    public float animationTime = 0.5f;
    public float leftLine; 
    public float rightLine; 

    private SpriteRenderer spriteRenderer;
    private bool isAnimating = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Ball.OnBallStopped += StartAnimation; 
    }

    void OnDestroy()
    {
        Ball.OnBallStopped -= StartAnimation; 
    }

    void StartAnimation(Vector2 ballPosition)
    {
        if (ballPosition.x > leftLine && ballPosition.x < rightLine && !isAnimating)
        {
            StartCoroutine(AnimateGoal());
        }
    }

    IEnumerator AnimateGoal()
    {
        isAnimating = true;
        spriteRenderer.sprite = sprite2;
        yield return new WaitForSeconds(animationTime);
        spriteRenderer.sprite = sprite1;
        isAnimating = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(leftLine, -100, 0), new Vector3(leftLine, 100, 0));
        Gizmos.DrawLine(new Vector3(rightLine, -100, 0), new Vector3(rightLine, 100, 0));
    }
}

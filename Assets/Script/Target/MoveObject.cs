using UnityEngine;
using DG.Tweening;

public class MoveObject : MonoBehaviour
{
    public float length = 5f;
    public float height = 5f;
    public float speed = 2f;
    public bool moveHorizontally;
    public bool moveVertically;
    public bool moveRandomly;

    private float minPositionX;
    private float maxPositionX;
    private float minPositionY;
    private float maxPositionY;

    private void Start()
    {
        minPositionX = transform.position.x - length / 2;
        maxPositionX = transform.position.x + length / 2;
        minPositionY = transform.position.y - height / 2;
        maxPositionY = transform.position.y + height / 2;
        Move();
    }

    private void Move()
    {
        if (moveRandomly)
        {
            if (Random.value > 0.5f)
                MoveHorizontally();
            else
                MoveVertically();
        }
        else if (moveHorizontally)
        {
            MoveHorizontally();
        }
        else if (moveVertically)
        {
            MoveVertically();
        }
    }

    private void MoveHorizontally()
    {
        float duration = length / speed;
        transform.DOMoveX(maxPositionX, duration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            transform.DOMoveX(minPositionX, duration).SetEase(Ease.InOutSine).OnComplete(Move);
        });
    }

    private void MoveVertically()
    {
        float duration = height / speed;
        transform.DOMoveY(maxPositionY, duration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            transform.DOMoveY(minPositionY, duration).SetEase(Ease.InOutSine).OnComplete(Move);
        });
    }
}

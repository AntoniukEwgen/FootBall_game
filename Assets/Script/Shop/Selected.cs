using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected : MonoBehaviour
{
    [System.Serializable]
    public struct SelectBall
    {
        public string nameBall;
        public Sprite sprite;
    }

    public SelectBall[] selectBalls;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        LoadData();
    }

    private void LoadData()
    {
        string savedName = PlayerPrefs.GetString("SelectedItem", "");

        foreach (var ball in selectBalls)
        {
            if (ball.nameBall == savedName)
            {
                spriteRenderer.sprite = ball.sprite;
                break;
            }
        }
    }
}

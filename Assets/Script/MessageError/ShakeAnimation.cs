using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class ShakeAnimation : MonoBehaviour, IPointerClickHandler
{
    public RectTransform target;
    public Button energyButton;
    public float shakeDuration = 1f;
    private bool isShaking = false;
    private float originalX;
    public GameObject MessageError;

    private void Start()
    {
        originalX = target.anchoredPosition.x;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!energyButton.interactable && !isShaking)
        {
            MessageError.SetActive(true);
            isShaking = true;
            target.DOShakePosition(shakeDuration, new Vector2(5, 0), 10, 90, false, true)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    target.anchoredPosition = new Vector2(originalX, target.anchoredPosition.y);
                    isShaking = false;
                });
        }
    }
}

using UnityEngine;
using DG.Tweening;

public class ScaleAnimation : MonoBehaviour
{
    public float animationTime = 1f; // Час анімації, який ви можете змінити в редакторі Unity
    public float delayTime = 1f; // Час затримки, який ви можете змінити в редакторі Unity

    void OnEnable()
    {
        // Збільшення масштабу з 0 до 1.5, а потім до 1
        transform.localScale = Vector3.zero; // Початковий масштаб
        transform.DOScale(0.5f, animationTime / 2).SetUpdate(true) // Збільшення масштабу до 1.5
            .OnComplete(() =>
            {
                transform.DOScale(0.3f, animationTime / 2).SetUpdate(true) // Зменшення масштабу до 1
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(delayTime, () =>
                    {
                        transform.DOScale(0, animationTime / 2).SetUpdate(true) // Зменшення масштабу до 0
                        .OnComplete(() => gameObject.SetActive(false)); // Вимкнення об'єкта
                    });
                });
            });
    }
}

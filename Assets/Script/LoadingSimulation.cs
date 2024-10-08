using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingSimulation : MonoBehaviour
{
    public Image loadingSlider; // замінено Image на Slider
    public TMP_Text percentageText;
    public TMP_Text loaderText;
    private float currentAmount = 0;
    private string loaderString = "Loading...";
    private int loaderIndex = 0;

    void Start()
    {
        StartCoroutine("LoadData");
        StartCoroutine("AnimateLoaderText");
    }

    IEnumerator LoadData()
    {
        while (currentAmount < 100)
        {
            currentAmount += Random.Range(10, 20);
            currentAmount = Mathf.Min(currentAmount, 100);
            loadingSlider.fillAmount = currentAmount / 100; 
            percentageText.text = $"{currentAmount}%";
            yield return new WaitForSeconds(Random.Range(1, 3));
        }
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator AnimateLoaderText()
    {
        while (true)
        {
            if (loaderIndex < loaderString.Length)
            {
                loaderText.text += loaderString[loaderIndex];
                loaderIndex++;
            }
            else
            {
                loaderText.text = "";
                loaderIndex = 0;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}

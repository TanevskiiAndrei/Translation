using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeArray : MonoBehaviour
{
    public float duration = 1.0f; // Длительность изменения альфы
    [SerializeField] private GameObject[] Image;
    [SerializeField] private GameObject[] Text;
    private Coroutine fadeCoroutine; // Одна корутина для всех панелей
    private bool isVisible = false; // Текущее состояние видимости

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Image.Length; i++)
        {
            Color color = Image[i].GetComponent<Image>().color;
            color.a = 0f; // Устанавливаем альфа-канал в 0
            Image[i].GetComponent<Image>().color = color; // Применяем изменения
        }
    }
    void OnEnable()
    {
        FadeON();
    }
    
    public void FadeON()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine); // Останавливаем текущую корутину
        }
        fadeCoroutine = StartCoroutine(FadeAllPanels(0f, 1f)); // Плавное появление для всех панелей
    }
    public void FadeOFF()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine); // Останавливаем текущую корутину
        }
        fadeCoroutine = StartCoroutine(FadeAllPanels(1f, 0f));
           StartCoroutine(Disactivation());
    }

    private IEnumerator Disactivation()
    {
        yield return new WaitForSeconds(1); 
        gameObject.SetActive(false);
    }

    private IEnumerator FadeAllPanels(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        // Сохраняем начальное значение альфы для всех изображений
        Color[] initialColors = new Color[Image.Length];
        for (int i = 0; i < Image.Length; i++)
        {
            initialColors[i] = Image[i].GetComponent<Image>().color;
            initialColors[i].a = startAlpha;
        }

        // Сохраняем начальное значение альфы для всех текстов, если они есть
        Color[] initialColorsText = null;
        if (Text != null)
        {
            initialColorsText = new Color[Text.Length];
            for (int i = 0; i < Text.Length; i++)
            {
                initialColorsText[i] = Text[i].GetComponent<Text>().color;
                initialColorsText[i].a = startAlpha;
            }
        }

        // Плавное изменение альфы всех изображений и текстов
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);

            for (int i = 0; i < Image.Length; i++)
            {
                Color color = initialColors[i];
                color.a = alpha;
                Image[i].GetComponent<Image>().color = color;
            }

            if (Text != null)
            {
                for (int i = 0; i < Text.Length; i++)
                {
                    Color color = initialColorsText[i];
                    color.a = alpha;
                    Text[i].GetComponent<Text>().color = color;
                }
            }

            yield return null;
        }

        // Убедитесь, что альфа точно установлена на конечное значение
        for (int i = 0; i < Image.Length; i++)
        {
            Color color = Image[i].GetComponent<Image>().color;
            color.a = endAlpha;
            Image[i].GetComponent<Image>().color = color;
        }

        if (Text != null)
        {
            for (int i = 0; i < Text.Length; i++)
            {
                Color color = Text[i].GetComponent<Text>().color;
                color.a = endAlpha;
                Text[i].GetComponent<Text>().color = color;
            }
        }
    }
}

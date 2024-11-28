using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CreatePanel : MonoBehaviour
{
    public float duration = 1.0f; // Длительность изменения альфы
    [SerializeField] private SerialPortReader _serialPortReader;
    [SerializeField] private GameObject[] Panel1;
    private Coroutine[] fadeCoroutines;
    private bool isVisible = false; // Текущее состояние видимости

    // Start is called before the first frame update
    void Start()
    {
        fadeCoroutines = new Coroutine[Panel1.Length];
        for (int i = 0; i < Panel1.Length; i++)
        {
            Color color = Panel1[i].GetComponent<Image>().color;
            color.a = 0f; // Устанавливаем альфа-канал в 0
            Panel1[i].GetComponent<Image>().color = color; // Применяем изменения
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (0 > _serialPortReader.EncoderValue && _serialPortReader.EncoderValue > -350)
        {
            // Плавное появление
            if (-107 >= _serialPortReader.EncoderValue && _serialPortReader.EncoderValue >= -115 && !isVisible)
            {
                isVisible = true;
                for (int i = 0; i < Panel1.Length; i++)
                {
                    if (fadeCoroutines[i] != null)
                    {
                        StopCoroutine(fadeCoroutines[i]); // Остановка текущей корутины
                    }
                    fadeCoroutines[i] = StartCoroutine(Fade(Panel1[i], 0f, 1f)); // Плавное появление
                }
            }
            // Плавное исчезновение
            else if (isVisible && !(-107 >= _serialPortReader.EncoderValue && _serialPortReader.EncoderValue >= -115))
            {
                isVisible = false;
                for (int i = 0; i < Panel1.Length; i++)
                {
                    if (fadeCoroutines[i] != null)
                    {
                        StopCoroutine(fadeCoroutines[i]); // Остановка текущей корутины
                    }
                    fadeCoroutines[i] = StartCoroutine(Fade(Panel1[i], 1f, 0f)); // Плавное исчезновение
                }
            }
        }
    }

    private IEnumerator Fade(GameObject panel, float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Image image = panel.GetComponent<Image>();
        Color color = image.color;

        // Применяем начальное значение альфы
        color.a = startAlpha;
        image.color = color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            color.a = alpha;
            image.color = color; // Применяем изменённый цвет
            yield return null;
        }

        // Убедитесь, что альфа точно установлена на конечное значение
        color.a = endAlpha;
        image.color = color; // Применяем конечный цвет
    }
}

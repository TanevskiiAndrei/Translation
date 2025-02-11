using System.Collections;
using UnityEngine;

public class EncoderController : MonoBehaviour
{
    [SerializeField] private SerialPortReader _serialPortReader;
    [SerializeField] private GameObject Panel_Rus;
    [SerializeField] private GameObject Panel_Tat;
    [SerializeField] private GameObject SelectPanel;
    [SerializeField] private GameObject TEXT_RUS;
    [SerializeField] private GameObject TEXT_TAT;
    public RectTransform uiObjects;
    public float moveSpeed = 1f;
    public float sensitivityThreshold = 0.1f;
    public float timerDuration = 3f;

    private float previousEncoderValue;
    private Coroutine timerCoroutine;
    private bool isPanelFading = false;
    private GameObject currentPanel;
    private bool previousIsRus;

    void Start()
    {
        previousEncoderValue = _serialPortReader.EncoderValue;
        uiObjects.anchoredPosition = new Vector2(0, 0);
        InitializeSelectPanel();
    }

    private void Update()
    {
        uiObjects.anchoredPosition = new Vector2(0, _serialPortReader.EncoderValue * moveSpeed);

        // Проверка изменения значения энкодера
        if (Mathf.Abs(previousEncoderValue - _serialPortReader.EncoderValue) > sensitivityThreshold)
        {
            if (!isPanelFading)
            {
                isPanelFading = true;
                SelectPanel.GetComponent<FadeArray>().FadeOFF();
            }
            ResetTimer();
        }
        else if (timerCoroutine == null)
        {
            timerCoroutine = StartCoroutine(StartTimer());
        }

        previousEncoderValue = _serialPortReader.EncoderValue;

        // Переключение текста при изменении языка
        if (previousIsRus != _serialPortReader.isRus)
        {
            SwitchText();
            previousIsRus = _serialPortReader.isRus;
        }

        // Переключение панели при изменении языка
        GameObject targetPanel = _serialPortReader.isRus ? Panel_Rus : Panel_Tat;
        if (currentPanel != targetPanel)
        {
            SwitchPanel(targetPanel);
        }
    }

    private void SwitchPanel(GameObject newPanel)
    {
        SelectPanel.SetActive(false);
        currentPanel = newPanel;
        SelectPanel = newPanel;

        SelectPanel.SetActive(true);
        SelectPanel.GetComponent<FadeArray>().FadeON();
        isPanelFading = false;
    }

    private void SwitchText()
    {
        /*if (_serialPortReader.isRus)
        {
            TEXT_RUS.SetActive(true);
            TEXT_TAT.SetActive(false);
        }
        else
        {
            TEXT_RUS.SetActive(false);
            TEXT_TAT.SetActive(true);
        }*/
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timerDuration);
        if (!SelectPanel.activeSelf)
        {
            SelectPanel.SetActive(true);
            SelectPanel.GetComponent<FadeArray>().FadeON();
        }
        isPanelFading = false;
        timerCoroutine = null;
    }

    private void ResetTimer()
    {
        StopTimer();
        timerCoroutine = StartCoroutine(StartTimer());
    }

    private void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    private void InitializeSelectPanel()
    {
        currentPanel = _serialPortReader.isRus ? Panel_Rus : Panel_Tat;
        SelectPanel = currentPanel;
        SelectPanel.SetActive(true);
        SwitchText();  // Инициализация правильного текста при старте
        previousIsRus = _serialPortReader.isRus;
    }
}

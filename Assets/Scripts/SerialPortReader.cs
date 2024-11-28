using System.IO.Ports;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

public class SerialPortReader : MonoBehaviour
{
    public event Action<float> OnEncoderValueChanged;
    public string portName = "COM3";
    public int baudRate = 9600;
    public int EncoderValue ;
    public int EncoderValueLast;
    public bool isRus = true;
    
    SerialPort serialPort;

    void Start()
    {
        Connect();
        EncoderValueLast = EncoderValue;
    }
    void Update()
    {
        ReadData();
    }

    void Connect()
    {
        serialPort = new SerialPort(portName, baudRate);
        try
        {
            serialPort.Open();
            Debug.Log("Подключение открыто");
        }
        catch (Exception e)
        {
            Debug.LogError("Ошибка: " + e);
        }
    }

    void ReadData()
    {
        // чтение данных из порта 

        string message = serialPort.ReadLine();
        print(message);

        if (message == "r")
        {
            isRus = true;
        }
        if (message == "t")
        {
            isRus = false;
        }
        // поиск числового значения
        Regex regex = new Regex(@"-?\d+");
        Match match = regex.Match(message);

        if (match.Success)
        {
            // преобразуем в int
            EncoderValue = int.Parse(match.Value);
        }
    }

    void Close()
    {
        if (serialPort != null)
        {
            serialPort.Close();
        }
    }

    void OnDestroy()
    {
        Close();
    }
}

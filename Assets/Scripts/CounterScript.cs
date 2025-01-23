using System;
using UnityEngine;
using UnityEngine.UI;

public class CounterScript : MonoBehaviour
{
    [SerializeField] private Text text; // текстовый элемент счетчика для отображения результата

    private int _count = 0;

    public class Settings // класс который будет использован для сериализации в json 
    {
        public int startingNumber = 0;
    }

    public void UpdateDataFromClass(Settings settings) // метод для обновления данных из скрипта загрузки
    {
        _count = settings.startingNumber;
        UpdateText();
    }
    
    public void IncreaseNumber() // метод вызываемый при нажатии кнопки "увеличить счетчик"
    {
        _count++;
        UpdateText();
    }
    
    private void UpdateText() 
    {
        text.text = _count.ToString();
    }
    private void OnDestroy()
    {
        LoadScript.SaveJsonCounter(new Settings
        {
            startingNumber = _count
        });
    }
}

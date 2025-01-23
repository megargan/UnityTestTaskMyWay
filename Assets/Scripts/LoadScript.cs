using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadScript : MonoBehaviour
{
    [SerializeField] private string bundlePath; // путь к бандлу который нужно загрузить
    [SerializeField] private string spriteName; // название файла со спрайтом для кнопки
    [SerializeField] private Button[] targetButtons; // кнопки к которым будет применен спрайт после загрузки 
    [SerializeField] private CounterScript counterobject; // обьект с скриптом счетчика на котором будут обновляться данные
    [SerializeField] private string settingsPath; //путь с json файлу с сохранением
    [SerializeField] private string messageSettingsPath; //путь с json файлу с текстом сообщения
    [SerializeField] private Text welcomeTextObject; // обьект на который будет загружен текст из файла
    
    void Start() // выполняем загрузку данных при запуске
    {
        StartCoroutine(LoadData());
    }
    
    public void UpdateButtonAction() // метод вызываемый при нажатии кнопки "обновить контент"
    {
        ShowLoadingScreen();
        StartCoroutine(LoadData());
    }
    
    
    private Sprite _sprite;
    IEnumerator LoadData()
    {
        yield return new WaitForSeconds(0.5f); // задержка для перехода в экран загрузки
        LoadJsonCounter();
        LoadJsonMessage();
        AssetBundle.UnloadAllAssetBundles(true); // выгрузить все ранее загруженные бандлы чтобы была возможность загрузить их снова
        
        Debug.Log("Starting load");
        AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);
        if (bundle == null)
        {
            Debug.Log("can not find asset bundle");
            yield break;
        }

        _sprite = bundle.LoadAsset<Sprite>(spriteName);
        if (_sprite == null)
        {
            Debug.Log("can not load sprite from bundle");
            yield break;
        }

        foreach (Button button in targetButtons)
        {
            button.image.sprite = _sprite;
        }
        
        Debug.Log("load success");

        yield return new WaitForSeconds(1f); // задержка в 1 секунду для реалистичной загрузки
        HideLoadingScreen();
        yield break;
    }
    
    
    void LoadJsonCounter() // подгузка счета из json файла
    {
        if (File.Exists(settingsPath))
        {
            string json = File.ReadAllText(settingsPath);
            CounterScript.Settings settings = JsonUtility.FromJson<CounterScript.Settings>(json);
            counterobject.UpdateDataFromClass(settings);
        }
        else // файл создается если его не существует
        {
            CounterScript.Settings settings = new CounterScript.Settings
            {
                startingNumber = 0
            };
            string newjson = JsonUtility.ToJson(settings);
            File.WriteAllText(settingsPath, newjson);
        }
    }

    void LoadJsonMessage()
    {
        if (File.Exists(messageSettingsPath))
        {
            string json = File.ReadAllText(messageSettingsPath);
            MessageJson messageJson = JsonUtility.FromJson<MessageJson>(json);
            welcomeTextObject.text = messageJson.welcometext;
        }
        else
        {
            MessageJson message = new MessageJson
            {
                welcometext = "Добро пожаловать в счетчик!"
            };
            string newjson = JsonUtility.ToJson(message);
            File.WriteAllText(messageSettingsPath, newjson);
        }
    }

    class MessageJson
    {
        public string welcometext;
    }
    
    
    
    
    #region LoadingScreenControll
    
    public CanvasGroup loadingScreenCanvasGroup; // canvasgroup загрузочного экрана
    public float fadeDuration = 0.5f; // сколько будет длиться плавное переключение экранов
    
    
    public void HideLoadingScreen() // выход из экрана загрузки
    {
        StartCoroutine(FadeOut());
    }

    public void ShowLoadingScreen() // вход в экран загрузки
    {
        StartCoroutine(FadeIn());
    }
    
    private IEnumerator FadeOut()
    {
        float startTime = Time.time;

        while (Time.time - startTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, (Time.time - startTime) / fadeDuration);
            loadingScreenCanvasGroup.alpha = alpha;
            yield return new WaitForSeconds(0.01f);
        }

        loadingScreenCanvasGroup.alpha = 0f;

        loadingScreenCanvasGroup.interactable = false;
        loadingScreenCanvasGroup.blocksRaycasts = false;
        Destroy(loadingScreenCanvasGroup.GetComponent<RotateImage>());
    }
    
    private IEnumerator FadeIn()
    {
        loadingScreenCanvasGroup.interactable = true;
        loadingScreenCanvasGroup.blocksRaycasts = true;
        
        float startTime = Time.time;

        while (Time.time - startTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, (Time.time - startTime) / fadeDuration);
            loadingScreenCanvasGroup.alpha = alpha;
            yield return new WaitForSeconds(0.01f);
        }
        loadingScreenCanvasGroup.alpha = 1f; 
    }
    #endregion
    
}

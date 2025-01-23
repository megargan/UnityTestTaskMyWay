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
    
    
    public void UpdateButtonAction() // метод вызываемый при нажатии кнопки "обновить контент"
    {
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
    
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadScript : MonoBehaviour
{
    [SerializeField] private string bundlepath; // путь к бандлу который нужно загрузить
    [SerializeField] private string spriteName; // название файла со спрайтом для кнопки
    [SerializeField] private Button[] targetButtons; // кнопки к которым будет применен спрайт после загрузки 
    
    
    public void UpdateButtonAction() // метод вызываемый при нажатии кнопки "обновить контент"
    {
        StartCoroutine(LoadData());
    }
    
    
    private Sprite sprite;
    IEnumerator LoadData()
    {
        yield return new WaitForSeconds(0.5f);
      
        AssetBundle.UnloadAllAssetBundles(true); // выгрузить все ранее загруженные бандлы чтобы была возможность загрузить их снова
        
        Debug.Log("Starting load");
        AssetBundle bundle = AssetBundle.LoadFromFile(bundlepath);
        if (bundle == null)
        {
            Debug.Log("can not find asset bundle");
            yield break;
        }

        sprite = bundle.LoadAsset<Sprite>(spriteName);
        if (sprite == null)
        {
            Debug.Log("can not load sprite from bundle");
            yield break;
        }

        foreach (Button button in targetButtons)
        {
            button.image.sprite = sprite;
        }
        
        Debug.Log("load success");

        yield return new WaitForSeconds(1f); // задержка в 1 секунду для реалистичной загрузки
       
        yield break;
    }
}

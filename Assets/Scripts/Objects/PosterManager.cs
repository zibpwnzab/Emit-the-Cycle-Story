using System.Collections;
using TMPro;
using UnityEngine;

public class PosterManager : MonoBehaviour
{
    public static PosterManager instance;
    public int collectedPosters;
      
    [SerializeField] private string textObjectName = "PosterText"; // Имя текстового объекта
    private TextMeshProUGUI posterText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }

    private void Start()
    {
        FindTextObject(); // Поиск текстового объекта при запуске
    }

    private void FindTextObject()
    {
        GameObject textObject = GameObject.Find(textObjectName);
        if (textObject != null)
        {
            posterText = textObject.GetComponent<TextMeshProUGUI>();
            posterText.gameObject.SetActive(false); // Деактивируем текст по умолчанию
        }
        else
        {
            Debug.LogWarning("Text object with the name " + textObjectName + " not found.");
        }
    }

    public void CollectPoster()
    {
        collectedPosters++;
        Debug.Log("Poster collected! Total posters: " + collectedPosters);
        if (posterText == null)
        {
            FindTextObject(); // Если текстовый объект потерян, попробуем найти его снова
        }

        if (posterText != null)
        {
            StartCoroutine(DisplayPosterText());
        }
    }

    private IEnumerator DisplayPosterText()
    {
        posterText.text = $"Подобрано {collectedPosters}/10 постеров";
        posterText.gameObject.SetActive(true);

        // Плавное отображение текста буква за буквой
        for (int i = 0; i <= posterText.text.Length; i++)
        {
            posterText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(0.05f); // Задержка между отображением каждой буквы
        }

        yield return new WaitForSeconds(3f); // Ждём 3 секунды перед скрытием текста
        posterText.gameObject.SetActive(false);
    }
}

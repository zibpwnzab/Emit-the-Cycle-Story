using System.Collections;
using TMPro;
using UnityEngine;

public class PosterManager : MonoBehaviour
{
    public static PosterManager instance;
    public int collectedPosters = 0;

    [SerializeField] private string textObjectName = "PosterText"; // Имя текстового объекта
    private TextMeshProUGUI posterText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad работает только для корневых объектов
            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else if (instance != this)
        {
            // Удаляем только компонент: объект общий с другими менеджерами
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
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
            if (posterText != null)
            {
                posterText.gameObject.SetActive(false); // Деактивируем текст по умолчанию
            }
        }
        else
        {
            Debug.LogWarning("Text object with the name " + textObjectName + " not found.");
        }
    }

    public void CollectPoster()
    {
        collectedPosters+= 1;
        Debug.Log("Poster collected! Total posters: " + collectedPosters);
        if (posterText == null)
        {
            FindTextObject(); // Если текстовый объект потерян, попробуем найти его снова
        }

        if (posterText != null)
        {
            StopAllCoroutines(); // Перезапуск анимации текста
            StartCoroutine(DisplayPosterText());
        }
    }

    // Сброс при смерти / новой игре
    public void ResetState()
    {
        collectedPosters = 0;
    }

    private IEnumerator DisplayPosterText()
    {
        posterText.text = $"Подобрано {collectedPosters}/10 постеров";
        posterText.gameObject.SetActive(true);

        // Плавное отображение текста буква за буквой
        for (int i = 0; i <= posterText.text.Length; i++)
        {
            if (posterText == null) yield break;
            posterText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(0.05f); // Задержка между отображением каждой буквы
        }

        yield return new WaitForSeconds(3f); // Ждём 3 секунды перед скрытием текста
        if (posterText != null) posterText.gameObject.SetActive(false);
    }
}

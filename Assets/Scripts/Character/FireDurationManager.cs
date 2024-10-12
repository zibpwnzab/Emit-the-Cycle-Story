using UnityEngine;
using TMPro; // Подключаем TextMeshPro
using UnityEngine.SceneManagement; // Подключаем SceneManager для работы с событиями сцены

public class FireManager : MonoBehaviour
{
    public static FireManager Instance { get; private set; }

    [SerializeField] private int totalFireCount = 3; // Общее количество фаеров
    private TMP_Text fireCountText; // Ссылка на UI-текст для отображения количества фаеров

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожать объект при смене сцен
            SceneManager.sceneLoaded += OnSceneLoaded; // Подписываемся на событие загрузки сцены
        }
        else
        {
            Destroy(gameObject); // Удалить дублирующий объект, если уже существует экземпляр
        }
    }

    private void Start()
    {
        FindFireCountText(); // Ищем текстовый объект на текущей сцене
        UpdateFireCountUI(); // Обновляем UI при запуске игры
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindFireCountText(); // Ищем текстовый объект при каждой загрузке новой сцены
        UpdateFireCountUI(); // Обновляем UI после загрузки новой сцены
    }

    private void FindFireCountText()
    {
        // Ищем текстовый объект по имени на сцене
        fireCountText = GameObject.Find("FireCountText")?.GetComponent<TMP_Text>();
    }

    public int GetTotalFireCount()
    {
        return totalFireCount;
    }

    public void DecreaseTotalFireCount()
    {
        if (totalFireCount > 0)
        {
            totalFireCount--;
            UpdateFireCountUI(); // Обновляем UI при изменении количества фаеров
        }
    }

    private void UpdateFireCountUI()
    {
        if (fireCountText != null)
        {
            fireCountText.text = $"X{totalFireCount}"; // Обновляем текст с количеством фаеров
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Отписываемся от события при уничтожении объекта
    }
}

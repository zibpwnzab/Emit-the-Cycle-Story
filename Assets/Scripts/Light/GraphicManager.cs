using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GraphicsSettingsManager : MonoBehaviour
{
    public static GraphicsSettingsManager Instance { get; private set; }

    [SerializeField] private GameObject graphicsSelectionCanvas; // Canvas с выбором графики
    [SerializeField] private GameObject inGameCanvas; // Canvas, который активируется после выбора настроек
    [SerializeField] private Button lowGraphicsButton;
    [SerializeField] private Button highGraphicsButton;

    private const string GraphicsQualityKey = "GraphicsQuality"; // Ключ для сохранения настроек

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожать объект при смене сцен
        }
        else
        {
            Destroy(gameObject); // Удалить дублирующий объект, если уже существует экземпляр
        }
    }

    private void Start()
    {
        // Подписка на событие загрузки новой сцены
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Проверяем сохраненные настройки и применяем их
        if (PlayerPrefs.HasKey(GraphicsQualityKey))
        {
            int savedQuality = PlayerPrefs.GetInt(GraphicsQualityKey);
            ApplyGraphicsSettings(savedQuality);
            ActivateInGameCanvas(); // Активируем основной Canvas
        }
        else
        {
            graphicsSelectionCanvas.SetActive(true); // Показываем выбор графики, если нет сохраненных настроек
        }

        // Устанавливаем слушатели для кнопок
        lowGraphicsButton.onClick.AddListener(() => SetGraphicsQuality(0));
        highGraphicsButton.onClick.AddListener(() => SetGraphicsQuality(1));
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Применяем настройки графики после загрузки сцены
        if (PlayerPrefs.HasKey(GraphicsQualityKey))
        {
            int savedQuality = PlayerPrefs.GetInt(GraphicsQualityKey);
            ApplyGraphicsSettings(savedQuality);
        }
    }

    private void SetGraphicsQuality(int qualityLevel)
    {
        PlayerPrefs.SetInt(GraphicsQualityKey, qualityLevel);
        PlayerPrefs.Save();

        ApplyGraphicsSettings(qualityLevel);

        // Отключаем Canvas с выбором графики и активируем основной Canvas
        graphicsSelectionCanvas.SetActive(false);
        ActivateInGameCanvas();
    }

    private void ApplyGraphicsSettings(int qualityLevel)
    {
        Light[] sceneLights = FindObjectsOfType<Light>();

        foreach (Light light in sceneLights)
        {
            var bakingOutput = light.bakingOutput;
            if (qualityLevel == 0) // Низкое качество: Запечённый свет
            {
                bakingOutput.lightmapBakeType = LightmapBakeType.Baked;
                light.bakingOutput = bakingOutput;
                light.shadows = LightShadows.None; // Отключаем динамические тени для низкого качества
            }
            else // Высокое качество: Реальное время
            {
                bakingOutput.lightmapBakeType = LightmapBakeType.Realtime;
                light.bakingOutput = bakingOutput;
                light.shadows = LightShadows.Soft; // Включаем динамические тени для высокого качества
            }
        }
    }

    private void ActivateInGameCanvas()
    {
        if (inGameCanvas != null)
        {
            inGameCanvas.SetActive(true); // Активируем Canvas, который используется во время игры
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Отписываемся от события при уничтожении объекта
    }
}

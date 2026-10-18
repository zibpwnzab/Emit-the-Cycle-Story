using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string DeathCutSceneName;
    [SerializeField] GameObject WinPanel;
    public static LevelManager Instance;
    private float currentTime;
    private bool _isFinishing;

    [SerializeField] UnityEngine.Events.UnityEvent OnDeathA;

    void Awake()
    {
        // В Awake, чтобы Instance был доступен в Start других скриптов
        if (!Instance) Instance = this;
    }

    void Start()
    {
        if (!Instance) Instance = this;

        // Получаем текущее время из PlayerPrefs, если нужно
        if (PlayerPrefs.HasKey(PlayerController.TOTAL_GAME_TIME))
        {
            // Ключ записывается через SetFloat
            currentTime = PlayerPrefs.GetFloat(PlayerController.TOTAL_GAME_TIME);
        }
        else
        {
            PlayerPrefs.SetFloat(PlayerController.TOTAL_GAME_TIME, 0);
            currentTime = 0;
        }
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void SetCarma(int carma)
    {
        // Устанавливаем карму через KarmaManager
        if (KarmaManager.Instance != null) KarmaManager.Instance.SetCarma(carma);
    }

    public void AddCarma(int carma)
    {
        // Добавляем карму через KarmaManager
        if (KarmaManager.Instance != null) KarmaManager.Instance.AddCarma(carma);
    }

    public int GetCarma()
    {
        // Получаем карму через KarmaManager
        return KarmaManager.Instance != null ? KarmaManager.Instance.GetCarma() : 0;
    }

    public void FinishLevel(bool win)
    {
        if (win)
        {
            Save();
            PlayerPrefs.SetInt(PlayerController.NEXT_LEVEL_KEY, UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
            if (WinPanel) WinPanel.SetActive(true);
        }
        else
        {
            // Защита от повторного вызова до загрузки сцены
            if (_isFinishing) return;
            _isFinishing = true;

            // Менеджеры в DontDestroyOnLoad не уничтожаем (на поздних уровнях их негде пересоздать), только сбрасываем
            if (KarmaManager.Instance != null) KarmaManager.Instance.ResetState();
            if (FireManager.Instance != null) FireManager.Instance.ResetState();
            if (PosterManager.instance != null) PosterManager.instance.ResetState();

            // Загружаем
            SceneManager.LoadScene(DeathCutSceneName);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Continue()
    {
        int nextLevel = PlayerPrefs.GetInt(PlayerController.NEXT_LEVEL_KEY);
        if (nextLevel < 0 || nextLevel >= SceneManager.sceneCountInBuildSettings)
        {
            nextLevel = 0;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevel);
    }

    public void Save()
    {
        // В LevelManager не нужно сохранять карму в PlayerPrefs, так как это теперь делает KarmaManager.
        // Если требуется сохранять дополнительные данные (например, время), их можно сохранять в PlayerPrefs.
        PlayerPrefs.Save();
    }
}

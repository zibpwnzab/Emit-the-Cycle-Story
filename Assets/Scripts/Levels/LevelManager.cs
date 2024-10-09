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

    [SerializeField] UnityEngine.Events.UnityEvent OnDeathA;

    void Start()
    {
        if (!Instance) Instance = this;

        // Получаем текущее время из PlayerPrefs, если нужно
        if (PlayerPrefs.HasKey(PlayerController.TOTAL_GAME_TIME))
        {
            currentTime = PlayerPrefs.GetInt(PlayerController.TOTAL_GAME_TIME);
        }
        else
        {
            PlayerPrefs.SetFloat(PlayerController.TOTAL_GAME_TIME, 0);
            currentTime = 0;
        }
    }

    public void SetCarma(int carma)
    {
        // Устанавливаем карму через KarmaManager
        KarmaManager.Instance.SetCarma(carma);
    }

    public void AddCarma(int carma)
    {
        // Добавляем карму через KarmaManager
        KarmaManager.Instance.AddCarma(carma);
    }

    public int GetCarma()
    {
        // Получаем карму через KarmaManager
        return KarmaManager.Instance.GetCarma();
    }

    public void FinishLevel(bool win)
    {
        if (win)
        {
            Save();
            PlayerPrefs.SetInt(PlayerController.NEXT_LEVEL_KEY, UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
            WinPanel.SetActive(true);
        }
        else
        {
            // Если игрок проиграл, сбрасываем карму через KarmaManager
            KarmaManager.Instance.SetCarma(0);

            // Удаляем все объекты, помеченные как DontDestroyOnLoad
            DestroyAllDontDestroyOnLoadObjects();

            // Загружаем
            SceneManager.LoadScene(DeathCutSceneName);
            UnityEngine.SceneManagement.SceneManager.LoadScene(DeathCutSceneName);
        }
    }
    private void DestroyAllDontDestroyOnLoadObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.scene.name == null || obj.scene.name == "DontDestroyOnLoad")
            {
                Destroy(obj);
            }
        }
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void Continue()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(PlayerPrefs.GetInt(PlayerController.NEXT_LEVEL_KEY));
    }

    public void Save()
    {
        // В LevelManager не нужно сохранять карму в PlayerPrefs, так как это теперь делает KarmaManager.
        // Если требуется сохранять дополнительные данные (например, время), их можно сохранять в PlayerPrefs.
        PlayerPrefs.Save();
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private string DeathCutSceneName;
    [SerializeField] GameObject WinPanel;
    public static LevelManager Instance;
    private int currentCarma;
    private float currentTime;

    [SerializeField] UnityEngine.Events.UnityEvent OnDeathA;
    void Start()
    {
        if (!Instance) Instance = this;
        if (PlayerPrefs.HasKey(PlayerController.PLAYER_CARMA_KEY))
        {
            currentCarma = PlayerPrefs.GetInt(PlayerController.PLAYER_CARMA_KEY);
            currentTime = PlayerPrefs.GetInt(PlayerController.TOTAL_GAME_TIME);
        }
        else
        {
            PlayerPrefs.SetInt(PlayerController.PLAYER_CARMA_KEY, 0);
            PlayerPrefs.SetFloat(PlayerController.TOTAL_GAME_TIME, 0);
            currentCarma = 0;
            currentTime = 0;
        }
    }


    public void SetCarma(int carma)
    {
        currentCarma = carma;
    }
    public void AddCarma(int carma)
    {
        currentCarma += carma;
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
            PlayerPrefs.SetInt(PlayerController.PLAYER_CARMA_KEY, 0);
            PlayerPrefs.SetInt(PlayerController.NEXT_LEVEL_KEY, 1);
            PlayerPrefs.SetInt(PlayerController.PLAYER_CARMA_KEY, 0);
            UnityEngine.SceneManagement.SceneManager.LoadScene(DeathCutSceneName);
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
        PlayerPrefs.SetInt(PlayerController.PLAYER_CARMA_KEY, currentCarma);

    }

    public int GetCarma()
    {
        return currentCarma;
    }
}
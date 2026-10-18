using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject ContinueButton;
    [SerializeField] TMPro.TMP_Text ContinueButtonText;
    [SerializeField] string FirstCutSceneName;
    [SerializeField] private GameObject exceptionObject;
    int nextLevel;

    private void Start()
    {
        if (PlayerPrefs.HasKey(PlayerController.NEXT_LEVEL_KEY))
        {
            nextLevel = PlayerPrefs.GetInt(PlayerController.NEXT_LEVEL_KEY);
        }
        else
        {
            nextLevel = 1;
            PlayerPrefs.SetInt(PlayerController.NEXT_LEVEL_KEY, 1);
        }

        if (nextLevel > 1)
        {
            if (ContinueButton)
            {
                ContinueButton.SetActive(true);
                if (ContinueButtonText)
                {
                    ContinueButtonText.text = string.Format(ContinueButtonText.text, nextLevel);
                }
            }
        }
    }

    public void NewGame()
    {
        // Сбрасываем только прогресс, настройки (графика, язык) сохраняем
        PlayerPrefs.DeleteKey(PlayerController.NEXT_LEVEL_KEY);
        PlayerPrefs.DeleteKey(PlayerController.TOTAL_GAME_TIME);
        PlayerPrefs.DeleteKey(PlayerController.PLAYER_CARMA_KEY);
        PlayerPrefs.DeleteKey(SaveGame.SavePrefsKey);
        PlayerPrefs.Save();

        // Менеджеры живут в DontDestroyOnLoad, сбрасываем их состояние
        if (KarmaManager.Instance != null) KarmaManager.Instance.ResetState();
        if (FireManager.Instance != null) FireManager.Instance.ResetState();
        if (PosterManager.instance != null) PosterManager.instance.ResetState();

        // Загружаем новую игру
        SceneManager.LoadScene(FirstCutSceneName);
    }

    public void ContinueGame()
    {
        if (nextLevel < 0 || nextLevel >= SceneManager.sceneCountInBuildSettings)
        {
            nextLevel = 1;
        }
        SceneManager.LoadScene(nextLevel);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}

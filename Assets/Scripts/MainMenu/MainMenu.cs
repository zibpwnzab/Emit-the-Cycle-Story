using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject ContinueButton;
    [SerializeField] TMPro.TMP_Text ContinueButtonText;
    [SerializeField] string FirstCutSceneName;
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
        // Сбрасываем все сохранённые данные (PlayerPrefs)
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Удаляем все объекты, помеченные как DontDestroyOnLoad
        DestroyAllDontDestroyOnLoadObjects();

        // Загружаем новую игру
        SceneManager.LoadScene(FirstCutSceneName);
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


    public void ContinueGame()
    {
        SceneManager.LoadScene(nextLevel);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}

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
        // Сбрасываем все сохранённые данные (PlayerPrefs)
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Найти объект GraphicManager, который не должен удаляться
        GameObject graphicManager = GameObject.Find("GraphicManager");

        // Удаляем все объекты, помеченные как DontDestroyOnLoad, кроме GraphicManager
        DestroyAllDontDestroyOnLoadObjectsExcept(graphicManager);

        // Загружаем новую игру
        SceneManager.LoadScene(FirstCutSceneName);
    }

    private void DestroyAllDontDestroyOnLoadObjectsExcept(GameObject exception)
    {
        var dontDestroyOnLoadObjects = new List<GameObject>();

        // Получаем все корневые объекты сцены
        //var sceneRoots = SceneManager.GetSceneByName("DontDestroyOnLoad").GetRootGameObjects();
        //dontDestroyOnLoadObjects.AddRange(sceneRoots);

        // Удаляем все объекты, кроме исключения (GraphicManager)
        foreach (var obj in dontDestroyOnLoadObjects)
        {
            if (obj != exception)
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

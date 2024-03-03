using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject ContinueButton;
    [SerializeField] TMPro.TMP_Text ContinueButtonText;
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
    public void NewGame() {
        SceneManager.LoadScene(1);
    }

    public void ContinueGame() {
        SceneManager.LoadScene(nextLevel);
    }

    public void QuitGame() {
        Application.Quit();
    }
}

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string DeathCutSceneName;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private UnityEvent OnDeathA;

    public static LevelManager Instance;

    private float _currentTime;
    private bool _isFinishing;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        if (Instance == null) Instance = this;

        if (PlayerPrefs.HasKey(PlayerController.TOTAL_GAME_TIME))
        {
            _currentTime = PlayerPrefs.GetFloat(PlayerController.TOTAL_GAME_TIME);
        }
        else
        {
            PlayerPrefs.SetFloat(PlayerController.TOTAL_GAME_TIME, 0f);
            _currentTime = 0f;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void SetCarma(int carma)
    {
        if (KarmaManager.Instance != null) KarmaManager.Instance.SetCarma(carma);
    }

    public void AddCarma(int carma)
    {
        if (KarmaManager.Instance != null) KarmaManager.Instance.AddCarma(carma);
    }

    public int GetCarma()
    {
        return KarmaManager.Instance != null ? KarmaManager.Instance.GetCarma() : 0;
    }

    public void FinishLevel(bool win)
    {
        if (win)
        {
            Save();
            PlayerPrefs.SetInt(
                PlayerController.NEXT_LEVEL_KEY,
                SceneManager.GetActiveScene().buildIndex + 1);

            if (WinPanel != null) WinPanel.SetActive(true);
        }
        else
        {
            if (_isFinishing) return;
            _isFinishing = true;

            if (KarmaManager.Instance != null) KarmaManager.Instance.ResetState();
            if (FireManager.Instance != null) FireManager.Instance.ResetState();
            if (PosterManager.instance != null) PosterManager.instance.ResetState();

            if (!string.IsNullOrEmpty(DeathCutSceneName))
            {
                SceneManager.LoadScene(DeathCutSceneName);
            }
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
        SceneManager.LoadScene(nextLevel);
    }

    public void Save()
    {
        PlayerPrefs.Save();
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FireManager : MonoBehaviour
{
    private const string FireCountTextObjectName = "FireCountText";

    public static FireManager Instance { get; private set; }

    [SerializeField] private int totalFireCount = 3;
    private TMP_Text _fireCountText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        RefreshSceneReferences();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshSceneReferences();
    }

    private void RefreshSceneReferences()
    {
        GameObject textObj = GameObject.Find(FireCountTextObjectName);
        _fireCountText = textObj != null ? textObj.GetComponent<TMP_Text>() : null;
        UpdateFireCountUI();
    }

    public int GetTotalFireCount() => totalFireCount;

    public void DecreaseTotalFireCount()
    {
        if (totalFireCount > 0)
        {
            totalFireCount--;
            UpdateFireCountUI();
        }
    }

    private void UpdateFireCountUI()
    {
        if (_fireCountText != null)
        {
            _fireCountText.text = $"X{totalFireCount}";
        }
    }
}
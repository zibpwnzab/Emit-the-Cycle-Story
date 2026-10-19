using UnityEngine;
using UnityEngine.SceneManagement;

public class KarmaManager : MonoBehaviour
{
    public static KarmaManager Instance { get; private set; }

    [SerializeField] private int currentCarma;
    private bool _carmaInitialized;

    // KarmaManager is placed only in LvL_1, so create one if a loaded scene has none
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void RegisterBootstrap()
    {
        SceneManager.sceneLoaded -= EnsureInstance;
        SceneManager.sceneLoaded += EnsureInstance;
    }

    private static void EnsureInstance(Scene scene, LoadSceneMode mode)
    {
        if (Instance == null)
        {
            new GameObject(nameof(KarmaManager)).AddComponent<KarmaManager>();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Only the component: the object is shared with FireManager and PosterManager
            Destroy(this);
        }
    }

    private void Start()
    {
        if (!_carmaInitialized)
        {
            currentCarma = 0;
            _carmaInitialized = true;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
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

    public int GetCarma() => currentCarma;

    public void ResetState()
    {
        currentCarma = 0;
        _carmaInitialized = true;
    }
}

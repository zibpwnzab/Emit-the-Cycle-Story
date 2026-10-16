using UnityEngine;

public class KarmaManager : MonoBehaviour
{
    public static KarmaManager Instance { get; private set; }

    [SerializeField] private int currentCarma;
    private bool _carmaInitialized;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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

    public void SetCarma(int carma)
    {
        currentCarma = carma;
    }

    public void AddCarma(int carma)
    {
        currentCarma += carma;
    }

    public int GetCarma() => currentCarma;
}
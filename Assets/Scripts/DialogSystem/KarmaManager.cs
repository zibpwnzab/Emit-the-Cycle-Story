using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarmaManager : MonoBehaviour
{
    public static KarmaManager Instance; // Singleton для доступа к объекту из других скриптов

    [SerializeField] private int currentCarma; // Отображение кармы в инспекторе (только для чтения)
    private bool carmaInitialized = false; // Флаг для контроля однократной инициализации кармы

    void Awake()
    {
        // Убедиться, что существует только один экземпляр KarmaManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожать объект при загрузке новых сцен
        }
        else
        {
            Destroy(gameObject); // Если уже есть экземпляр, уничтожаем новый
            return; // Прекращаем выполнение Awake() для нового объекта
        }
    }

    void Start()
    {
        if (!carmaInitialized)
        {
            // Инициализация кармы только один раз
            currentCarma = 0; // Устанавливаем начальное значение кармы
            carmaInitialized = true;
            Debug.Log($"Карма инициализирована: {currentCarma}");
        }
    }

    public void SetCarma(int carma)
    {
        currentCarma = carma;
        Debug.Log($"Карма установлена: {currentCarma}");
    }

    public void AddCarma(int carma)
    {
        currentCarma += carma;
        Debug.Log($"Карма изменена: {currentCarma}");
    }

    public int GetCarma()
    {
        return currentCarma;
    }
}

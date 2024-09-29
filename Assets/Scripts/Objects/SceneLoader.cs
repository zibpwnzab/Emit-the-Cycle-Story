using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, IInteractable
{
    [SerializeField] private List<ISignal> signalSources; // Список сигналов
    [SerializeField] private string sceneName; // Загрузка сцены по имени
    [SerializeField] private bool needsSignal = true; // Требуются ли сигналы для загрузки
    [SerializeField] private bool useTrigger = true; // Чекбокс для активации через триггер
    [SerializeField] private bool useTimer = false; // Чекбокс для активации через таймер
    [SerializeField] private float delayBeforeLoad = 5.0f; // Задержка перед загрузкой сцены

    private bool sceneLoadingTriggered = false; // Флаг, чтобы избежать повторной загрузки

    private void Start()
    {
        // Если таймер активирован, запускаем его
        if (useTimer)
        {
            StartCoroutine(LoadSceneAfterDelay());
        }
    }

    // Метод загрузки сцены по истечении таймера
    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        LoadNextScene(null); // Удалим игрока перед загрузкой
    }

    // Метод, который вызывается при прохождении через триггер
    private void OnTriggerEnter(Collider other)
    {
        if (useTrigger && other.CompareTag("Player")) // Проверяем, что объект — это игрок
        {
            LoadNextScene(other.gameObject); // Удалим игрока перед загрузкой
        }
    }

    public bool StopInteraction(GameObject gameObject, Animator animator)
    {
        return Interact(gameObject, animator);
    }

    public bool Interact(GameObject gameObject, Animator animator)
    {
        if (needsSignal && !AreAllSignalsActive())
        {
            return false; // Если хотя бы один сигнал не активен, не загружаем сцену
        }

        LoadNextScene(gameObject); // Удалим игрока перед загрузкой
        return true;
    }

    // Метод проверки всех сигналов
    private bool AreAllSignalsActive()
    {
        foreach (var signal in signalSources)
        {
            if (!signal.Signal()) // Проверяем каждый сигнал
            {
                return false; // Если хотя бы один сигнал не активен, возвращаем false
            }
        }
        return true; // Все сигналы активны
    }

    private void LoadNextScene(GameObject player)
    {
        if (sceneLoadingTriggered)
            return;

        sceneLoadingTriggered = true;

        // Удаляем объект игрока перед загрузкой новой сцены
        if (player != null)
        {
            Destroy(player);
        }

        Debug.Log("Загружаем сцену: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    // Метод для загрузки сцены по нажатию кнопки UI
    public void LoadSceneFromUIButton()
    {
        if (sceneLoadingTriggered)
            return; // Избегаем повторной загрузки

        if (needsSignal && !AreAllSignalsActive())
        {
            Debug.Log("Не все сигналы активны, сцена не загружена.");
            return; // Если сигналы нужны и не все активны, не загружаем сцену
        }

        // Если не требуется проверка сигналов или все сигналы активны, загружаем сцену
        Debug.Log("Загружаем сцену через кнопку UI: " + sceneName);
        LoadNextScene(null); // Загружаем сцену (вызвано из UI кнопки)
    }
}

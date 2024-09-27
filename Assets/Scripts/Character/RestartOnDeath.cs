using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject player; // Игрок, за которым следим
    [SerializeField] private float delayBeforeRestart = 2.0f; // Задержка перед перезапуском сцены

    private bool isRestarting = false; // Чтобы избежать повторных перезапусков

    void Update()
    {
        if (player == null && !isRestarting) // Проверяем, жив ли игрок
        {
            StartCoroutine(RestartScene()); // Если игрок мертв, запускаем перезапуск
        }
    }

    // Перезапуск сцены с задержкой
    private IEnumerator RestartScene()
    {
        isRestarting = true;
        yield return new WaitForSeconds(delayBeforeRestart); // Задержка перед перезапуском
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Перезапуск текущей сцены
    }
}

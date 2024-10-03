using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PosterInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite posterImage; // Картинка постера
    [SerializeField] private GameObject imageDisplay; // UI объект для отображения изображения
    [SerializeField] private Image imageComponent; // UI элемент для картинки
    [SerializeField] private float interactionDelay = 2f; // Время задержки перед возможностью закрыть постер
    private bool isPosterOpen = false; // Проверка, открыт ли постер
    private bool canClick = true; // Флаг для блокировки кликов

    private void Start()
    {
        Debug.Log("PosterInteractable initialized on " + gameObject.name);
    }

    public bool Interact(GameObject player, Animator playerAnimator)
    {
        if (isPosterOpen || !canClick) // Если постер открыт или клики заблокированы
            return false;

        Debug.Log("Player interacted with poster: " + gameObject.name);
        StartCoroutine(ShowPoster());
        return true;
    }

    public bool StopInteraction(GameObject gameObject, Animator animator)
    {
        return false; // Не требуется завершение взаимодействия
    }

    // Метод для показа постера с задержкой перед закрытием
    private IEnumerator ShowPoster()
    {
        isPosterOpen = true;
        canClick = false; // Блокируем клики

        // Показать изображение постера
        imageComponent.sprite = posterImage;
        imageDisplay.SetActive(true);
        Debug.Log("Poster is displayed.");

        // Ждем перед тем, как разрешить закрытие постера
        yield return new WaitForSeconds(interactionDelay);

        Debug.Log("Poster can now be closed.");

        // Ожидание нажатия на экран для закрытия постера
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        // Закрыть постер
        ClosePoster();
    }

    private void ClosePoster()
    {
        Debug.Log("Poster is closed.");
        imageDisplay.SetActive(false);
        isPosterOpen = false;
        canClick = true; // Снова разрешаем клики

        // Увеличиваем количество собранных постеров
        PosterManager.instance.CollectPoster(); // <-- вызов менеджера постеров

        // Перемещаем объект вниз на 20 единиц по оси Y
        transform.position += new Vector3(0, -20, 0);
        Debug.Log("Poster object moved down: " + gameObject.name);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PosterInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite posterImage; // Картинка постера
    [SerializeField] private GameObject imageDisplay; // UI объект для отображения изображения
    [SerializeField] private Image imageComponent; // UI элемент для картинки
    [SerializeField] private float interactionDelay = 2f; // Время задержки перед возможностью закрыть постер
    private Animator animator;
    private bool isPosterOpen = false; // Проверка, открыт ли постер

    private void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log("PosterInteractable initialized on " + gameObject.name);
    }

    public bool Interact(GameObject player, Animator playerAnimator)
    {
        if (isPosterOpen)
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

        // Показать изображение постера
        imageComponent.sprite = posterImage;
        imageDisplay.SetActive(true);
        Debug.Log("Poster is displayed.");

        // Блокируем управление игроком на несколько секунд
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

        // Увеличиваем количество собранных постеров
        PosterManager.instance.CollectPoster();  // <-- добавлен вызов менеджера постеров

        // Отключаем объект
        gameObject.SetActive(false);
        Debug.Log("Poster object is disabled: " + gameObject.name);
    }
}

/*using UnityEngine;

public class TriggerAction : MonoBehaviour
{
    public float moveDistance = 5f; // Расстояние, на которое перемещается игрок по X
    public string animationName = "YourAnimation"; // Название анимации, которую нужно запустить
    public float animationDuration = 1.5f; // Длительность анимации (для задержки перед перемещением)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Отключение управления
                //playerController.DisableControl();
                // Запуск анимации
                playerController.PlayAnimation(animationName);
                // Перенос игрока вперед после задержки
                StartCoroutine(MovePlayerAfterDelay(playerController, animationDuration, moveDistance));
            }
        }
    }

    private IEnumerator MovePlayerAfterDelay(PlayerController playerController, float delay, float distance)
    {
        yield return new WaitForSeconds(delay);
        playerController.MoveForward(distance);
        // Включение управления (если необходимо)
        playerController.EnableControl();
    }
}
*/
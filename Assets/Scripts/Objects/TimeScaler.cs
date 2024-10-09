using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    [Range(0f, 5f)]
    [SerializeField] private float timeScale = 1f; // Настраиваемая шкала времени в инспекторе

    private void OnValidate()
    {
        // Проверка, чтобы обновить Time.timeScale при изменении значения в инспекторе
        UpdateTimeScale();
    }

    private void UpdateTimeScale()
    {
        Time.timeScale = timeScale; // Устанавливаем Time.timeScale на значение из инспектора
    }

    private void OnEnable()
    {
        UpdateTimeScale(); // Устанавливаем Time.timeScale при активации объекта
    }

    private void OnDisable()
    {
        Time.timeScale = 1f; // Сбрасываем Time.timeScale на 1 при отключении объекта
    }
}

using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    [Range(0f, 5f)]
    [SerializeField] private float timeScale = 1f; // Настраиваемая шкала времени в инспекторе

    private void OnValidate()
    {
        // Проверка, чтобы обновить Time.timeScale при изменении значения в инспекторе
        // (только в Play Mode)
        if (Application.isPlaying && isActiveAndEnabled) UpdateTimeScale();
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
        // Сбрасываем Time.timeScale на 1 при отключении объекта, если это не пауза
        if (Mathf.Approximately(Time.timeScale, timeScale)) Time.timeScale = 1f;
    }
}

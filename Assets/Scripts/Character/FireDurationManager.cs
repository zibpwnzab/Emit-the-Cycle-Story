using UnityEngine;
using System;
using System.Collections;


public class FireDurationManager : MonoBehaviour
{
    public event Action OnFireTimeExpired; // Событие, вызываемое при истечении времени

    [SerializeField] private float totalFireDuration = 6f; // Общее время работы фаера
    private float remainingFireTime; // Оставшееся время работы фаера
    private bool isFireActive = false;

    private void Start()
    {
        remainingFireTime = totalFireDuration;
    }

    public void StartFireTimer()
    {
        if (!isFireActive)
        {
            remainingFireTime = totalFireDuration;
            isFireActive = true;
            StartCoroutine(FireDurationCountdown());
        }
    }

    public void StopFireTimer()
    {
        isFireActive = false;
        remainingFireTime = 0;
    }

    private IEnumerator FireDurationCountdown()
    {
        while (remainingFireTime > 0 && isFireActive)
        {
            remainingFireTime -= Time.deltaTime;
            yield return null;
        }

        if (remainingFireTime <= 0)
        {
            isFireActive = false;
            OnFireTimeExpired?.Invoke(); // Запускаем событие окончания времени
        }
    }

    public float GetRemainingTime()
    {
        return remainingFireTime;
    }

    public float GetTotalFireDuration()
    {
        return totalFireDuration;
    }
}

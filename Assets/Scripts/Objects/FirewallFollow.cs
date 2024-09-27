using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectCoroutine : MonoBehaviour
{
    [SerializeField] private List<Transform> points; // Список точек для перемещения
    [SerializeField] private float speed = 1.0f; // Скорость перемещения

    private int currentPointIndex = 0; // Индекс текущей точки

    void Start()
    {
        if (points.Count > 0)
        {
            // Запускаем перемещение, если есть хотя бы одна точка
            StartCoroutine(MoveObject());
        }
        else
        {
            Debug.LogWarning("Точки для перемещения не заданы!");
        }
    }

    IEnumerator MoveObject()
    {
        while (true)
        {
            Transform targetPoint = points[currentPointIndex];

            // Двигаем объект к текущей точке
            while (Vector3.Distance(transform.position, targetPoint.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
                yield return null;
            }

            // Переходим к следующей точке
            currentPointIndex = (currentPointIndex + 1) % points.Count;

            // Делаем паузу между перемещениями (если нужно)
            yield return new WaitForSeconds(0.5f); // Задержка 0.5 секунд перед следующим перемещением
        }
    }
}
    
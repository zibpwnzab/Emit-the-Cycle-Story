using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] public LaserReceiver signal;
    [SerializeField] public GameObject door;
    [SerializeField] public GameObject[] plane;
    [SerializeField] public GameObject dust;
    private float y;

    void Start()
    {
        // Инициализируем переменную y текущим положением двери по оси Y
        y = transform.position.y;
    }

    void Update()
    {
        // Если сигнал активен и дверь еще не поднялась на максимальную высоту
        if (signal.powered && y < 10)
        {
            // Включаем эффект пыли
            dust.gameObject.SetActive(true);

            // Меняем цвет всех объектов в массиве plane
            foreach (GameObject obj in plane)
            {
                Renderer planeRenderer = obj.GetComponent<Renderer>();
                planeRenderer.material.color = new Color(0, 255, 0);
            }

            // Увеличиваем позицию двери по оси Y
            y += 0.05f;

            // Обновляем позицию двери с сохранением ее координат X и Z
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
    }
}

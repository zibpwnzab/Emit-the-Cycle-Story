using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] public LaserReceiver signal;
    [SerializeField] public GameObject door;
    [SerializeField] public GameObject[] plane;
    [SerializeField] public GameObject dust;
    // Скорость подъёма двери, ед./с
    [SerializeField] private float openSpeed = 3f;
    private float y;
    private bool _planesColored;

    void Start()
    {
        // Инициализируем переменную y текущим положением двери по оси Y
        y = transform.position.y;
    }

    void Update()
    {
        // Если сигнал активен и дверь еще не поднялась на максимальную высоту
        if (signal != null && signal.powered && y < 10)
        {
            // Включаем эффект пыли
            if (dust != null) dust.SetActive(true);

            // Меняем цвет всех объектов в массиве plane (один раз)
            if (!_planesColored && plane != null)
            {
                _planesColored = true;
                foreach (GameObject obj in plane)
                {
                    if (obj != null && obj.TryGetComponent(out Renderer planeRenderer))
                    {
                        planeRenderer.material.color = new Color(0, 255, 0);
                    }
                }
            }

            // Увеличиваем позицию двери по оси Y
            y += openSpeed * Time.deltaTime;

            // Обновляем позицию двери с сохранением ее координат X и Z
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
    }
}

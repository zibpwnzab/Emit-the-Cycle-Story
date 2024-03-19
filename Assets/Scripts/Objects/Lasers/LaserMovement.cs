using UnityEngine;

public class LaserMovement : MonoBehaviour
{
    public float speed = 5f; // Скорость движения лазера
    public float maxHeight = 10f; // Максимальная высота
    public float minHeight = 0f; // Минимальная высота
    private bool movingUp = true; // Начальное направление движения

    // Update вызывается на каждом кадре
    void Update()
    {
        // Проверяем, в каком направлении двигаться
        if (transform.position.y >= maxHeight)
        {
            movingUp = false;
        }
        else if (transform.position.y <= minHeight)
        {
            movingUp = true;
        }

        // Выполняем движение вверх или вниз в зависимости от направления
        if (movingUp)
        {
            // Движение вверх
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else
        {
            // Движение вниз
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }
}

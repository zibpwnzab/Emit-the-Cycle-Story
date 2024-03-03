using UnityEngine;

public class RotateSpotlight : MonoBehaviour
{
    public float rotationSpeed = 50.0f; // Скорость вращения

    private void Update()
    {
        // Получаем текущий угол поворота по оси Y
        float currentRotation = transform.eulerAngles.y;

        // Вычисляем новый угол поворота
        float newRotation = currentRotation + rotationSpeed * Time.deltaTime;

        // Применяем новый угол поворота к оси Y
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, newRotation, transform.eulerAngles.z);
    }
}

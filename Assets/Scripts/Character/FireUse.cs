using System.Collections;
using UnityEngine;

public class FireUse : MonoBehaviour
{
    [SerializeField] private GameObject fireVFX; // Префаб VFX
    [SerializeField] private Animator fireAnimator; // Аниматор для фаера
    [SerializeField] private Light fireLight; // Источник света фаера
    [SerializeField] private float lightDuration = 2f; // Время, на которое свет активен

    // Метод для использования фаера
    public void UseFire()
    {
        Debug.Log("Fire used!");
        StartCoroutine(ActivateFire());
    }

    private IEnumerator ActivateFire()
    {
        // Включаем VFX
        if (fireVFX != null)
        {
            fireVFX.SetActive(true);
        }

        // Запускаем анимацию
        if (fireAnimator != null)
        {
            fireAnimator.SetTrigger("Activate");
        }

        // Включаем свет
        if (fireLight != null)
        {
            fireLight.enabled = true;
            yield return new WaitForSeconds(lightDuration);
            fireLight.enabled = false; // Выключаем свет после завершения
        }

        // Отключаем VFX (если нужно)
        if (fireVFX != null)
        {
            fireVFX.SetActive(false);
        }
    }
}

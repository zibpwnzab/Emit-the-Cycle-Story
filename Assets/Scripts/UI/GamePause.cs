using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    public GameObject panel;
    public List<GameObject> objectsToDisable; // Список объектов, которые нужно отключать при паузе

    public void pause()
    {
        Time.timeScale = 0;
        panel.SetActive(true);
        DisableObjects(); // Отключаем объекты при паузе
    }

    public void resume()
    {
        Time.timeScale = 1;
        panel.SetActive(false);
        EnableObjects(); // Включаем объекты при снятии паузы
    }

    private void DisableObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
    }

    private void EnableObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(true);
        }
    }
}
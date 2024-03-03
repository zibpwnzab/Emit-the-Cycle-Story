using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    public GameObject panel;
    public void pause() 
    {
        Time.timeScale = 0;
        panel.SetActive(true);
    }

    public void resume()
    { 
        Time.timeScale = 1;
        panel.SetActive(false);

    }
}

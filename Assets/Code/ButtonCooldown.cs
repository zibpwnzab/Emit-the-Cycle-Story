using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Buttoncooldown : MonoBehaviour
{
    public Image imageCooldown;
    public float cooldown = 10;
    bool isCooldown;
    bool isCliked;
    public Light light;


    public void TaskOnClick()
    {
        isCliked = true;
    }

    void Update()
    {

        if (isCliked)
        {
            isCooldown = true;
            light.enabled = true;
        }

        if (isCooldown)
        {
            
            imageCooldown.fillAmount += 1 / cooldown * Time.deltaTime;

            if (imageCooldown.fillAmount >= 0.02) 
            { 
            light.enabled = false;      
            }

            if (imageCooldown.fillAmount >= 1) 
            {
                imageCooldown.fillAmount = 0;
                isCooldown = false;
            }

            isCliked= false; 
        }

    }


}

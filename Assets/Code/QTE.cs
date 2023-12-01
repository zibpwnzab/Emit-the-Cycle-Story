using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTE : MonoBehaviour
{
    public Slider slider;
    
    
    
    void Start()
    {
         slider.value = 0f;
    }

    private void Update()
    {
        StartCoroutine(ValueMinus());
    }
    public void ValueChange()
    {
         slider.value += 1;
    }

    private IEnumerator ValueMinus() 
    {
        yield return new WaitForSeconds(2);

        if (slider.value != 10 && slider.value !=0)
        {
            slider.value -= 0.01f;
        }

    }
}

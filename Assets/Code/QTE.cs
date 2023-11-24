using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTE : MonoBehaviour
{
    private ValueType value;
    
    void Start()
    {
        value = GameObject.Find("Slider").GetComponent<ValueType>();
    }

    
    void Update()
    {
        
    }
}

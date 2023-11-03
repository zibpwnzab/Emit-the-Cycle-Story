using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TriggerObj : MonoBehaviour
{

    public bool onTrigger = false;


    private void OnTriggerEnter(Collider collision)
    {
        onTrigger = true;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public bool inAction = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        { 
        inAction = true;
        }
    }

}

using cherrydev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    private PlayerController player;
    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player) 
        {
            GameController.instance.EventAnimTrigger();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (player) 
        {
            
        }
    }

}

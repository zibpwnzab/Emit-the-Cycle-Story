using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public bool triger;
    private PlayerController player;
    private void Start()
    {
      player = GameObject.Find("Player").GetComponent<PlayerController>();
        
    }

    private void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        
        
    }
        
    

    private void OnTriggerExit(Collider other)
    {
        
    }
}

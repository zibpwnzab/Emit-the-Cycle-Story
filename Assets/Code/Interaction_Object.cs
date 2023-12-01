using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Interaction_Object: MonoBehaviour
{
    [SerializeField] private bool onTrigger = false;
    public Button button;
    private InteractionButton interactionButton;
    public bool interactionEnabled = false;

    void Start() 
    {
        interactionButton = GameObject.Find("InteractionButton").GetComponent<InteractionButton>();

    }

    private void Update()
    {
        Interact();
    }

    private void OnTriggerEnter(Collider collision)
    {
        button.interactable = true;
        onTrigger = true;
        
    }

    private void OnTriggerExit(Collider other)
    {
        button.interactable= false;
        onTrigger = false;
    }

    public void Interact() 
    {
        if (interactionButton.isPressed && onTrigger)
        {
            interactionEnabled = true;
            Debug.Log("mmmm");
            
        }
        interactionButton.isPressed = false;
    }

}
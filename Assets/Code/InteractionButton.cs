using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class InteractionButton : MonoBehaviour
{
    private TriggerObj triggerObj;
   public  bool isClicked;
    void Start()
    {
        triggerObj = GameObject.Find("TriggerObj").GetComponent<TriggerObj>();

    }

    public void OnClick() 
    { 
        isClicked = true;
    }

    private void Update()
    {
        if (triggerObj.onTrigger) 
        {
            if(isClicked)
            Debug.Log("Interaction");
        }
    }
}
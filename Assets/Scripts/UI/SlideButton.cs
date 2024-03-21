using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlideButton : MonoBehaviour, IPointerClickHandler
{


    public bool isPressed = false;


    public void OnPointerClick(PointerEventData eventData)
    {
        isPressed = true;
    }

}


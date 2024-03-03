using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class InteractionButton : MonoBehaviour
{
    public bool isPressed = false;

    public void Click()
    {
        isPressed = true;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class JumpButton : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private Rigidbody rigidbody;
    public bool isPressed = false;
    

    public void OnPointerClick(PointerEventData eventData)
    {
        isPressed = true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class JumpButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
#if UNITY_EDITOR
        if (Input.GetKey("space"))
        {
        FindObjectOfType<PlayerController>().StartCoroutine("Jump");
        }

#endif
        FindObjectOfType<PlayerController>().StartCoroutine("Jump");
    }

}

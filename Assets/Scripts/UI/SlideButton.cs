using cherrydev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SlideButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftControl))
        {
        FindObjectOfType<PlayerController>().StartCoroutine("Slide");
        }
#endif
        FindObjectOfType<PlayerController>().StartCoroutine("Slide");
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectCoroutine : MonoBehaviour
{
    [SerializeField] private Transform pointA; 
    [SerializeField] private Transform pointB; 
    [SerializeField] private float speed = 1.0f; 

    void Start()
    {
        StartCoroutine(MoveObject());
    }

    IEnumerator MoveObject()
    {
        while (Vector3.Distance(transform.position, pointB.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    Rigidbody _rigidbody;
    [SerializeField] float speed;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new();
        input += Vector3.forward * Input.GetAxis("Vertical");
        input += Vector3.right * Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.LeftShift)) input += Vector3.up;
        if (Input.GetKey(KeyCode.LeftControl)) input += Vector3.down;

        _rigidbody.velocity = Vector3.ClampMagnitude(input * speed + _rigidbody.velocity, speed);
    }
}

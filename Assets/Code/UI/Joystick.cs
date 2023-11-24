using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick_Controller : MonoBehaviour
{
    public float speed;
    public Joystick Joystick;
    public Rigidbody2D rb;
    public Joystick joystick;
    private Vector2 MoveVelocity;

    void Start()
    {
        GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (joystick.Horizontal > 0) ;
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (joystick.Horizontal < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }



        Vector2 moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        MoveVelocity = moveInput.normalized * speed;
    }

    public void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * Joystick.Vertical + Vector3.right * Joystick.Horizontal;
        rb.AddForce(direction * speed * Time.fixedDeltaTime);

    }
}
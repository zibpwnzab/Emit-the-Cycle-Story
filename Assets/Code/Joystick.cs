using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public float speed;

    Rigidbody2D rb;

    public Joystick joystick;

    private Vector2 MoveVelocity;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
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

    private void FixedUodate()
    {
        rb.MovePosition(rb.position + MoveVelocity * Time.deltaTime);
    }
}
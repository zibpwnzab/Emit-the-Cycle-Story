using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpforce = 4;
    private JumpButton jumpButton;
    private bool isJumping = false;
    public int Lifes = 3;
    private Interaction_Object interactionAnim;
    


    void Start()
    {
        
        jumpButton = GameObject.Find("JumpButton").GetComponent<JumpButton>();
        //interactionAnim = GameObject.Find("SM_Wep_Crowbar_01").GetComponent<Interaction_Object>();
        animator = GetComponent<Animator>();
       
    }

    void Update()
    {
        StartCoroutine(Jump());
        //Interaction();
        rigidbody.angularVelocity = Vector3.zero;
        
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector3(joystick.Horizontal * moveSpeed, rigidbody.velocity.y, joystick.Vertical * moveSpeed);

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
            animator.SetFloat("Speed", Vector3.ClampMagnitude(rigidbody.velocity, 1).magnitude);
        }
        else
            animator.SetFloat("Speed", Vector3.ClampMagnitude(rigidbody.velocity, 0).magnitude);
    }

    private IEnumerator Jump()
    {
        yield return new WaitForSeconds(1f);
        if (jumpButton.isPressed && !isJumping)
        {
            animator.SetTrigger("Jumping");
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpforce, rigidbody.velocity.z);
            
            isJumping = true;
        }
        jumpButton.isPressed = false;

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }


    }

    /*void Interaction()
    {
        if (interactionAnim.interactionEnabled)
        {
            animator.SetTrigger("Gather");
            //rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
            Debug.Log("hh");

        }
        interactionAnim.interactionEnabled = false;

    }*/

    private void PlayerDie() 
    {
        if (Lifes == 0) 
        {
        
        }
    }
}
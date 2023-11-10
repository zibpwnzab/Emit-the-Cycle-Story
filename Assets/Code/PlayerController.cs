using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpforce = 5f;
    private JumpButton jumpButton;
    private bool isJumping = false;


    void Start()
    {
        jumpButton = GameObject.Find("BtnJump").GetComponent<JumpButton>();
#if UNITY_EDITOR
        UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
#endif
    }

    void Update()
    {
        
        Jump();
        
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

    public void Jump()
    {
        if (jumpButton.isPressed && !isJumping)
        {
            animator.SetTrigger("Jumping");
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpforce, rigidbody.velocity.z);
            isJumping = true;
         
        }
        
        jumpButton.isPressed = false;
        isJumping = false;

    }
}
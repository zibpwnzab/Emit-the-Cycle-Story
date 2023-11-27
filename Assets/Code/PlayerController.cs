using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Animator animator;
    [SerializeField] public float moveSpeed;
    [SerializeField] private float jumpforce = 5;
    [SerializeField] private Button interactButton;
    private JumpButton jumpButton;
    private bool isJumping = false;
    private Interaction_Object interactionAnim;
    private List<IInteractable> interactables;


    void Start()
    {
        interactables = new();
        jumpButton = GameObject.FindObjectOfType<JumpButton>();
        //interactionAnim = GameObject.Find("SM_Wep_Crowbar_01").GetComponent<Interaction_Object>();
        animator = GetComponent<Animator>();
       
    }

    void Update()
    {
        
        //Jump();
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

    void Jump()
    {
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

    void Interaction()
    {
        if (interactionAnim.interactionEnabled)
        {
            animator.SetTrigger("Gather");
            //rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;


        }
        interactionAnim.interactionEnabled = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable obj;
        if (!other.TryGetComponent(out obj))
            return;

        if (interactables.Contains(obj))
            return;

        interactables.Add(obj);
        ResolveInteraction();
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable obj;
        if (!other.TryGetComponent(out obj))
            return;

        if (!interactables.Contains(obj))
            return;

        interactables.Remove(obj);
        ResolveInteraction();
    }

    void ResolveInteraction()
    {
        interactButton.onClick.RemoveAllListeners();
        if (interactables.Count == 0)
        {

        }
        else
        {
            interactButton.onClick.AddListener(delegate { interactables[0].Interact(gameObject, animator); });
        }
    }
}
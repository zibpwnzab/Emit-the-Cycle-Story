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
    [SerializeField] public float movingObjectModifier;
    [SerializeField] public float rotateObjectSpeed;
    [SerializeField] private float jumpforce = 5;
    [SerializeField] private Button interactButton;
    public int Lifes = 3;
    [SerializeField] JumpButton jumpButton;
    private bool isJumping = false;
    private Interaction_Object interactionAnim;
    private List<IInteractable> interactables;
    private GameObject lastInteracteble;
    public PlayerState playerState = PlayerState.Walking;



    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        interactables = new();
        jumpButton = GameObject.FindObjectOfType<JumpButton>();
        //interactionAnim = GameObject.Find("SM_Wep_Crowbar_01").GetComponent<Interaction_Object>();
        animator = GetComponent<Animator>();

    }

    void Update()
    {

        Jump();
        //Interaction();
        rigidbody.angularVelocity = Vector3.zero;

    }

    private void FixedUpdate()
    {
        switch (playerState)
        {
            case PlayerState.Walking:
                Move();
                break;
            case PlayerState.MovingObject:
                MoveWithObject();
                break;
            case PlayerState.RotatingObject:
                RotateObject();
                break;

        }

        void Move()
        {
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;
            Vector3 forwardDir = new Vector3(forward.x, 0, forward.z).normalized;
            Vector3 rightDir = new Vector3(right.x, 0, right.z).normalized;

            if (joystick.Vertical > 0)
            {
                rigidbody.AddForce(forwardDir * moveSpeed * Time.deltaTime,ForceMode.VelocityChange);
            }

            if (joystick.Horizontal < 0)
            {
                rigidbody.AddForce(rightDir * -moveSpeed * Time.deltaTime,ForceMode.VelocityChange);
            }
            if (joystick.Vertical < 0)
            {
                rigidbody.AddForce(forwardDir * -moveSpeed * Time.deltaTime,ForceMode.VelocityChange);
            }

            if (joystick.Horizontal > 0)
            {
                rigidbody.AddForce(rightDir * moveSpeed * Time.deltaTime,ForceMode.VelocityChange);
            }

            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                transform.rotation = Quaternion.LookRotation(rigidbody.velocity - Vector3.up * rigidbody.velocity.y);
                animator.SetFloat("Speed", Vector3.ClampMagnitude(rigidbody.velocity, 1).magnitude);
            }
            else
                animator.SetFloat("Speed", Vector3.ClampMagnitude(rigidbody.velocity, 0).magnitude);
        }
    }

    void RotateObject()
    {
        if (Mathf.Abs(joystick.Horizontal) > Mathf.Abs(joystick.Vertical))
        {
            Debug.Log(lastInteracteble.name + " Rotate");
            lastInteracteble.transform.Rotate(Vector3.up, Time.deltaTime * joystick.Horizontal * rotateObjectSpeed);
        }
        else
        {

        }
    }
    void Move()
    {     
          rigidbody.AddForce(joystick.Horizontal * moveSpeed, 0f, joystick.Vertical * moveSpeed);
        

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(rigidbody.velocity - Vector3.up * rigidbody.velocity.y);
            animator.SetFloat("Speed", Vector3.ClampMagnitude(rigidbody.velocity, 1).magnitude);
        }
        else
            animator.SetFloat("Speed", Vector3.ClampMagnitude(rigidbody.velocity, 0).magnitude);
    }

    void MoveWithObject()
    {
        rigidbody.velocity = new Vector3(joystick.Horizontal * moveSpeed * movingObjectModifier, rigidbody.velocity.y, joystick.Vertical * moveSpeed * movingObjectModifier);

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
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
        if (lastInteracteble == null)
            lastInteracteble = other.gameObject;
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

        if (other.gameObject == lastInteracteble)
            lastInteracteble = null;
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

    public void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}

public enum PlayerState
{
    Walking,
    Dead,
    MovingObject,
    RotatingObject,
}

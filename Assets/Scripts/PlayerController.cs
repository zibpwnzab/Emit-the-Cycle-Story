using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Animator animator;
    private float runSpeedThreshold = 4.0f;
    private float moveSpeed = 5.5f;
    [SerializeField] public float movingObjectModifier;
    [SerializeField] public float rotateObjectSpeed;
    [SerializeField] private float jumpforce = 5;
    [SerializeField] private Button interactButton;
    [SerializeField] SlideButton slideButton;
    public float slideSpeed = 5f;
    public float slideDistance = 5f;
    private bool isSliding = false;
    private Vector3 targetPosition;
    public float fallThreshold = 10f;
    public float climbSpeed = 5f;
    private bool isClimbing = false;
    public static PlayerController instance;

    public int Lifes = 3;
    [SerializeField] JumpButton jumpButton;
    [SerializeField]private bool isJumping = false;
    private List<IInteractable> interactables;
    private GameObject lastInteracteble;
    public PlayerState playerState = PlayerState.Walking;

    public static string PLAYER_CARMA_KEY = "PLAYER_CARMA_KEY";
    public static string NEXT_LEVEL_KEY = "NEXT_LEVEL_KEY";
    public static string TOTAL_GAME_TIME = "TOTAL_GAME_TIME";


    [SerializeField] private TMPro.TMP_Text VelocityText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        interactables = new();
        jumpButton = FindObjectOfType<JumpButton>();
        slideButton = FindObjectOfType<SlideButton>();
        animator = GetComponent<Animator>();

    }

    void Update()
    {
#if UNITY_EDITOR
        if (VelocityText) VelocityText.text = rigidbody.velocity.ToString("F2");
#endif
        Jump();
        rigidbody.angularVelocity = Vector3.zero;
        IsPlayerFalling();
    }
    public void ForceKick(Vector3 direction, float stunTime)
    {
        
        if (lastInteracteble) if (lastInteracteble.TryGetComponent(out IInteractable interactable))
        {
            interactable.StopInteraction(gameObject, animator);
        }

        rigidbody.velocity = direction;
        StartCoroutine(StunRecover(stunTime));
        
        
    }
    public void ForceKick(Vector3 direction)
    {
        ForceKick(direction, 1);
    }
   

    private void FixedUpdate()
    {
        switch (playerState)
        {
            case PlayerState.Walking:
                MoveOld();
                break;
            case PlayerState.MovingObject:
                MoveWithObject();
                break;
            case PlayerState.RotatingObject:
                RotateObject();
                break;
        }
        
        
    }
    IEnumerator StunRecover(float stun)
    {
        animator.SetBool("Stuned", true); 
        moveSpeed = 3.0f; 
        yield return new WaitForSeconds(stun);
        playerState = PlayerState.Walking;
        animator.SetBool("Stuned", false); 
        moveSpeed = 5.5f; 
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

void MoveOld()
{
    Vector3 desiredVelocity = ChangeVectorWRTCamera(new Vector3(joystick.Horizontal * moveSpeed, rigidbody.velocity.y, joystick.Vertical * moveSpeed));
    rigidbody.velocity = desiredVelocity;


        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
    {

        Vector3 lookDirection = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        transform.rotation = Quaternion.LookRotation(lookDirection);


        float currentSpeed = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z).magnitude;

        if (!isJumping)
        {
            animator.SetFloat("Speed", Vector3.ClampMagnitude(rigidbody.velocity, 1).magnitude);

            if (currentSpeed > runSpeedThreshold)
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }
        }
    }
    else
    {
        if (!isJumping)
        {
            animator.SetFloat("Speed", 0);
            animator.SetBool("isRunning", false);
        }
    }

}

    void MoveWithObject()
    {

        Vector3 newVelocity = ChangeVectorWRTCamera(new Vector3(joystick.Horizontal * moveSpeed * movingObjectModifier, rigidbody.velocity.y, joystick.Vertical * moveSpeed * movingObjectModifier));
        rigidbody.velocity = newVelocity;

        bool isMoving = joystick.Horizontal != 0 || joystick.Vertical != 0;


        if (playerState == PlayerState.MovingObject && isMoving)
        {
            animator.SetBool("Pushing", true);
            animator.SetFloat("Speed", Vector3.ClampMagnitude(newVelocity, 1).magnitude);
        }
        else
        {

            animator.SetBool("Pushing", false);
            animator.SetFloat("Speed", 0); 
        }
    }



    Vector3 ChangeVectorWRTCamera(Vector3 direction)
    {
        var camera = Camera.main;

        var forward = camera.transform.forward;
        var right = camera.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        var desiredMoveDirection = forward * direction.z + right * direction.x;
        desiredMoveDirection.y = direction.y;
        return desiredMoveDirection;
    }

void Jump()
    {
#if UNITY_EDITOR
        if (Input.GetKey("space") && !isJumping)
        {
            animator.SetTrigger("Jumping");
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpforce, rigidbody.velocity.z);
            isJumping = true;
            jumpButton.isPressed = false;
        }

#endif
        if (jumpButton.isPressed && !isJumping)
        {
            animator.SetTrigger("Jumping");
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpforce, rigidbody.velocity.z);
            isJumping = true;
            jumpButton.isPressed = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }

    }

    public IEnumerator Slide() 
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftControl) && !isSliding && animator.GetBool("isRunning"))
        { 
                animator.SetTrigger("Sliding");
            Vector3 slideDirection = transform.forward;
            targetPosition = transform.position + transform.forward * slideDistance;
            rigidbody.MovePosition(transform.position + slideDirection * slideSpeed * Time.deltaTime);
            isSliding = true;
            if (Vector3.Distance(transform.position, slideDirection * slideDistance) < 0.1f)
            {
                isSliding = false;
                animator.SetFloat("Speed", Vector3.ClampMagnitude(rigidbody.velocity, 0).magnitude);
            }
        }
#endif
            animator.SetTrigger("Sliding");
            Vector3 slideDirection = transform.forward;
            targetPosition = transform.position + transform.forward * slideDistance;
            rigidbody.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, slideSpeed * Time.deltaTime));
            isSliding = true; 

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isSliding = false;
                rigidbody.velocity = Vector3.zero;
                animator.SetFloat("Speed", Vector3.ClampMagnitude(rigidbody.velocity, 0).magnitude);

            }
        yield return null;
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
        LevelManager.Instance.FinishLevel(false);
    }

    private void IsPlayerFalling()
    {
        float heightChange = rigidbody.velocity.y;

        if (heightChange < -fallThreshold)
        {
            StartCoroutine(StunRecover(5.0f));
        }
    }




}


public enum PlayerState
{
    Walking,
    Dead,
    MovingObject,
    RotatingObject,
    Stunned,
}



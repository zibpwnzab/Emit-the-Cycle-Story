using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] public float moveSpeed;
    [SerializeField] public float movingObjectModifier;
    [SerializeField] public float rotateObjectSpeed;
    [SerializeField] private float jumpforce = 5;
    [SerializeField] private Button interactButton;
    [SerializeField] SlideButton slideButton;
    public float slideSpeed = 5f;
    public float slideDistance = 5f;
    private bool isSliding = false;
    private Vector3 slideDirection = Vector3.zero;


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

    void Start()
    {
        if (!rigidbody)rigidbody = GetComponent<Rigidbody>();
        interactables = new();
        jumpButton = FindObjectOfType<JumpButton>();
        slideButton = FindObjectOfType<SlideButton>();
        if (!animator) animator = GetComponent<Animator>();

    }

    void Update()
    {
#if UNITY_EDITOR
        if (VelocityText) VelocityText.text = rigidbody.velocity.ToString("F2");
#endif
            Slide();
            JumpAnim();
            rigidbody.angularVelocity = Vector3.zero;

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
    IEnumerator StunRecover(float stun)
    {
        playerState = PlayerState.Stunned;
        yield return new WaitForSeconds(stun);
        playerState = PlayerState.Walking;
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
            case PlayerState.Stunned:
                break;
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
    void MoveOld()
    {
        if (playerState == PlayerState.Stunned) return;

         rigidbody.velocity = ChangeVectorWRTCamera(new Vector3(joystick.Horizontal * moveSpeed, rigidbody.velocity.y, joystick.Vertical * moveSpeed));
        

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(rigidbody.velocity - Vector3.up * rigidbody.velocity.y);
            if (!isJumping)
            animator.SetFloat("Speed", Vector3.ClampMagnitude(rigidbody.velocity, 1).magnitude);
        }
        else
            if (!isJumping) animator.SetFloat("Speed", Vector3.ClampMagnitude(rigidbody.velocity, 0).magnitude);
    }

    void MoveWithObject()
    {
        rigidbody.velocity = ChangeVectorWRTCamera(new Vector3(joystick.Horizontal * moveSpeed * movingObjectModifier, rigidbody.velocity.y, joystick.Vertical * moveSpeed * movingObjectModifier));

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            animator.SetFloat("Speed", Vector3.ClampMagnitude(rigidbody.velocity, 1).magnitude);
        }
        else
            animator.SetFloat("Speed", Vector3.ClampMagnitude(rigidbody.velocity, 0).magnitude);
    }

    Vector3 ChangeVectorWRTCamera(Vector3 direction)
    {

        //assuming we only using the single camera:
        var camera = Camera.main;

        //camera forward and right vectors:
        var forward = camera.transform.forward;
        var right = camera.transform.right;

        //project forward and right vectors on the horizontal plane (y = 0)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        //this is the direction in the world space we want to move:
        var desiredMoveDirection = forward * direction.z + right * direction.x;
        desiredMoveDirection.y = direction.y;
        return desiredMoveDirection;
    }

    void JumpAnim() 
    {
#if UNITY_EDITOR
        if (Input.GetKey("space")) 
        {
            animator.SetTrigger("Jumping");
            Jump();
            isJumping = true;
        }

#endif
        if (jumpButton.isPressed && !isJumping)
        {
            animator.SetTrigger("Jumping");
            Jump();
            isJumping = true;
        }
                
    }
void Jump()
    {
        Vector3 vector3 = new Vector3(rigidbody.velocity.x, jumpforce, rigidbody.velocity.z);
        rigidbody.velocity = vector3;

        isJumping = false;
        jumpButton.isPressed = false;

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }


    }

    void Slide() 
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftControl))
        {
            animator.SetTrigger("Sliding");
            Vector3 slideDirection = transform.forward;
            rigidbody.MovePosition(transform.position + slideDirection * slideSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, slideDirection * slideDistance) < 0.1f)
            {
                isSliding = false;
            }
        }
#endif
        if (slideButton.isPressed && !isSliding) 
        {
            animator.SetTrigger("Sliding");
            Vector3 slideDirection = transform.forward;
            rigidbody.MovePosition(transform.position + slideDirection * slideSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, slideDirection * slideDistance) < 0.1f)
            {
                isSliding = false;
            }
        }
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
}

public enum PlayerState
{
    Walking,
    Dead,
    MovingObject,
    RotatingObject,
    Stunned,
}

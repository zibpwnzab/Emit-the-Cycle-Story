using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] private Button fireButton; // Кнопка для активации фаера
    [SerializeField] private GameObject fireVFX; // Префаб VFX фаера
    [SerializeField] private Light fireLight; // Источник света для фаера
    [SerializeField] private float fireLightDuration = 2f; // Длительность света
    private bool isUsingFire = false; // Проверка, активирован ли фаер
    private bool isJumping = false;
    private List<IInteractable> interactables;
    private GameObject lastInteractable;
    public PlayerState playerState = PlayerState.Walking;
    public static PlayerController instance;
    public int Lifes = 3;
    public float fallThreshold = 10f; // Минимальная скорость падения, при которой будет считаться, что игрок падает

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
        interactables = new List<IInteractable>();
        animator = GetComponent<Animator>();
        // Привязываем событие нажатия кнопки фаера
        fireButton.onClick.AddListener(ToggleFire);
    }


    void Update()
    {
#if UNITY_EDITOR
        if (VelocityText) VelocityText.text = rigidbody.velocity.ToString("F2");
#endif
        rigidbody.angularVelocity = Vector3.zero;
        IsPlayerFalling();
    }

    private void ToggleFire()
    {
        isUsingFire = !isUsingFire;

        if (isUsingFire)
        {
            // Включаем VFX
            if (fireVFX != null)
            {
                fireVFX.SetActive(true);
            }

            // Запускаем анимацию экипирования факела
            animator.SetTrigger("FireUse");

            // Включаем свет
            if (fireLight != null)
            {
                fireLight.enabled = true;
            }

            // Устанавливаем флаг бега с факелом
            StartCoroutine(StartRunningWithTorchAfterEquip());
        }
        else
        {
            // Отключаем VFX
            if (fireVFX != null)
            {
                fireVFX.SetActive(false);
            }

            // Отключаем свет
            if (fireLight != null)
            {
                fireLight.enabled = false;
            }

            // Сбрасываем флаг бега с факелом
            animator.SetBool("isRunning", false);
        }
    }

    private IEnumerator StartRunningWithTorchAfterEquip()
    {
        // Ждём окончания анимации экипирования факела (например, 1 секунда)
        yield return new WaitForSeconds(0f);

        // Устанавливаем флаг для перехода в состояние бега с факелом
        animator.SetBool("isRunning", true);
    }





    public void ForceKick(Vector3 direction, float stunTime)
    {
        Debug.Log("ForceKick вызван с направлением: " + direction);

        if (lastInteractable && lastInteractable.TryGetComponent(out IInteractable interactable))
        {
            interactable.StopInteraction(gameObject, animator);
        }

        if (rigidbody != null)
        {
            rigidbody.velocity = direction;
        }
        else
        {
            Debug.LogWarning("Rigidbody отсутствует на " + gameObject.name);
        }

        StartCoroutine(StunRecover(stunTime));
    }

    public void TakeDamage(int damage)
    {
        Lifes -= damage;
        if (Lifes <= 0)
        {
            Die();
        }
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
            Debug.Log(lastInteractable.name + " Rotate");
            lastInteractable.transform.Rotate(Vector3.up, Time.deltaTime * joystick.Horizontal * rotateObjectSpeed);
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
            Debug.Log("FF");
            //animator.SetFloat("Speed", Vector3.ClampMagnitude(newVelocity, 1).magnitude);
        }
        else
        {

            animator.SetBool("Pushing", false);
            //animator.SetFloat("Speed", 0); 
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

    public IEnumerator Jump()
    {
        if (!isJumping)
        {
            animator.SetTrigger("Jumping");
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpforce, rigidbody.velocity.z);
            isJumping = true;

        }
        yield return null;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }

    }


    




    private void OnTriggerEnter(Collider other)
    {
        IInteractable obj;
        if (!other.TryGetComponent(out obj))
            return;

        if (interactables.Contains(obj))
            return;
        if (lastInteractable == null)
            lastInteractable = other.gameObject;
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

        if (other.gameObject == lastInteractable)
            lastInteractable = null;
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



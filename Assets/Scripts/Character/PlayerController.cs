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
    [SerializeField] SlideButton slideButton;
    public float slideSpeed = 5f; // Настраиваемая скорость подката
    public float slideDistance = 5f; // Настраиваемая дистанция подката
    private bool isSliding = false;
    private Vector3 targetPosition;
    public float fallThreshold = 10f;
    public float climbSpeed = 5f;
    private bool isClimbing = false;
    public static PlayerController instance;
    public bool isMovingRight = false;
    private bool isInvulnerable = false;
    public float invulnerabilityTime = 5f;

    public int Lifes = 3;
    [SerializeField] JumpButton jumpButton;
    [SerializeField] private bool isJumping = false;
    private List<IInteractable> interactables;
    private GameObject lastInteractable;
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
        rigidbody.angularVelocity = Vector3.zero;
        IsPlayerFalling();
        // Проверяем нажатие Ctrl и запуск подката
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isSliding && animator.GetBool("isRunning"))
        {
            StartCoroutine(Slide());
        }
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
        if (!isInvulnerable)
        {
            Lifes -= damage;
            if (Lifes <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(InvulnerabilityCoroutine());
            }
        }
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
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
            isMovingRight = true;


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


    public IEnumerator Slide()
    {
        if (isSliding)
            yield break;

        // Отключаем джойстик для блокировки управления
        joystick.enabled = false;

        // Триггерим анимацию подката
        animator.SetTrigger("Sliding");
        isSliding = true;

        // Рассчитываем конечную позицию подката
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + transform.forward * slideDistance;

        // Определяем переменную для управления временем и скоростью подката
        float currentSlideDistance = 0f;
        float minDistanceBeforeStop = 0.05f; // Минимальное расстояние для завершения подката

        while (currentSlideDistance < slideDistance)
        {
            // Проверяем, есть ли препятствие на пути
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.1f, transform.forward, out hit, 0.5f))
            {
                // Останавливаем подкат, если на пути есть препятствие
                break;
            }

            // Плавное движение вперед
            Vector3 nextPosition = Vector3.MoveTowards(transform.position, targetPosition, slideSpeed * Time.fixedDeltaTime);

            // Применяем новое положение
            rigidbody.MovePosition(nextPosition);

            // Увеличиваем пройденное расстояние
            currentSlideDistance += (nextPosition - transform.position).magnitude;

            // Ожидаем следующее обновление физики
            yield return new WaitForFixedUpdate();
        }

        // Завершаем подкат
        isSliding = false;
        rigidbody.velocity = Vector3.zero;
        animator.SetFloat("Speed", 0);

        // Включаем джойстик обратно
        joystick.enabled = true;
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



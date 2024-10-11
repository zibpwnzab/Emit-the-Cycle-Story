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
    [SerializeField] private float runSpeedThreshold = 2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] public float movingObjectModifier;
    [SerializeField] public float rotateObjectSpeed;
    [SerializeField] private float jumpforce = 5;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button fireButton; // Кнопка для активации фаера
    [SerializeField] private Button buttonToDisable1; // Первая кнопка, отключаемая во время использования фаера
    [SerializeField] private Button buttonToDisable2; // Вторая кнопка, отключаемая во время использования фаера
    [SerializeField] private GameObject fireVFX; // Префаб VFX фаера
    [SerializeField] private Light fireLight; // Источник света для фаера
    [SerializeField] private GameObject fire3DModel; // 3D-модель, которая будет активироваться
    [SerializeField] private float fireActivationDelay = 0.5f; // Задержка перед активацией фаера

    // Поля для звуков
    [SerializeField] private AudioClip activationSound; // Звук активации фаера
    [SerializeField] private AudioClip fireLoopSound; // Звук, проигрывающийся во время использования фаера
    [SerializeField] private AudioClip deactivationSound; // Звук деактивации фаера
    private AudioSource audioSource; // Аудиоисточник для проигрывания звуков

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
        audioSource = gameObject.AddComponent<AudioSource>(); // Создаем компонент AudioSource
    }


    void Update()
    {
#if UNITY_EDITOR
        if (VelocityText) VelocityText.text = rigidbody.velocity.ToString("F2");
#endif
        rigidbody.angularVelocity = Vector3.zero;
        IsPlayerFalling();

        // Проверяем состояние движения игрока
        if (animator.GetBool("FireUse"))
        {
            if (rigidbody.velocity.magnitude > 0.1f)
            {
                // Если игрок двигается, устанавливаем состояние бега с факелом
                animator.SetBool("isRunning", true);
            }
            else
            {
                // Если игрок остановился, возвращаемся в состояние стояния с факелом
                animator.SetBool("isRunning", false);
            }
        }
    }

    private void ToggleFire()
    {
        isUsingFire = !isUsingFire;

        // Устанавливаем значение булевого параметра FireUse в зависимости от состояния isUsingFire
        animator.SetBool("FireUse", isUsingFire);

        if (isUsingFire)
        {
            // Запускаем корутину для задержки перед активацией
            StartCoroutine(ActivateFireWithDelay());

            // Отключаем кнопки
            if (buttonToDisable1 != null) buttonToDisable1.interactable = false;
            if (buttonToDisable2 != null) buttonToDisable2.interactable = false;

            // Проигрываем звук активации с задержкой 2 секунды
            StartCoroutine(PlaySoundWithDelay(activationSound, 1.7f, 2.5f));
        }
        else
        {
            // Отключаем все эффекты немедленно
            DeactivateFireEffects();

            // Включаем кнопки обратно
            if (buttonToDisable1 != null) buttonToDisable1.interactable = true;
            if (buttonToDisable2 != null) buttonToDisable2.interactable = true;

            // Проигрываем звук деактивации с запуском после 4 секунд звука
            StartCoroutine(PlaySoundWithDelay(deactivationSound, 0f, 3f, 4f));
        }
    }

    private IEnumerator ActivateFireWithDelay()
    {
        // Ждем указанное время перед активацией
        yield return new WaitForSeconds(fireActivationDelay);

        // Включаем VFX
        if (fireVFX != null)
        {
            fireVFX.SetActive(true);
        }

        // Включаем свет
        if (fireLight != null)
        {
            fireLight.enabled = true;
        }

        // Включаем 3D-модель
        if (fire3DModel != null)
        {
            fire3DModel.SetActive(true);
        }

        // Запускаем звук использования фаера (постоянный)
        if (fireLoopSound != null)
        {
            audioSource.clip = fireLoopSound;
            audioSource.loop = true;
            StartCoroutine(PlaySoundWithDelay(fireLoopSound, 0f, 200f, 0f));
            audioSource.Play();
        }
    }

    private void DeactivateFireEffects()
    {
        // Останавливаем звук использования фаера
        audioSource.Stop();

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

        // Отключаем 3D-модель
        if (fire3DModel != null)
        {
            fire3DModel.SetActive(false);
        }
    }

    private IEnumerator PlaySoundWithDelay(AudioClip clip, float startDelay, float duration, float startTime = 0f)
    {
        yield return new WaitForSeconds(startDelay);

        if (clip != null)
        {
            audioSource.time = startTime; // Устанавливаем начальную точку воспроизведения
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(duration);
            audioSource.Stop();
        }
    }






    private IEnumerator StartRunningWithTorchAfterEquip()
    {
        // Ждём окончания анимации экипирования факела (например, 1 секунда)
        yield return new WaitForSeconds(1.0f);

        // Устанавливаем флаг, что персонаж держит факел
        animator.SetBool("isHoldingTorch", true);

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



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject fireVFX;
    [SerializeField] private Light fireLight;
    [SerializeField] private GameObject fire3DModel;
    [SerializeField] private Button fireButton; // Кнопка для активации фаера
    [SerializeField] private float fireActivationDelay = 01.5f; // Задержка перед активацией фаера
    [SerializeField] private float fireTotalDuration = 6f; // Общее время работы фаера
     [SerializeField]private float fireRemainingTime; // Оставшееся время работы фаера
    [SerializeField] private int totalFireCount = 3; // Общее количество фаеров, доступных игроку
    [SerializeField] private float rechargeTime = 10f; // Время перезарядки после использования фаера

    // UI элементы
    [SerializeField] private GameObject[] fireUIElements; // Массив для хранения трёх UI элементов

    // Кнопки, которые будут отключаться во время использования фаера
    [SerializeField] private Button buttonToDisable1;
    [SerializeField] private Button buttonToDisable2;

    // Поля для объектов со звуком
    [SerializeField] private GameObject activationSoundObject;
    [SerializeField] private GameObject fireLoopSoundObject;
    [SerializeField] private GameObject deactivationSoundObject;

    private bool isUsingFire = false;
    private bool fireDepleting = false; // Флаг, отслеживающий истощение фаера
    private bool isRecharging = false; // Флаг, указывающий на процесс перезарядки

    
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
         SceneManager.sceneLoaded += (scene, mode) => OnSceneLoaded();
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        interactables = new List<IInteractable>();
        animator = GetComponent<Animator>();
        // Привязываем событие нажатия кнопки фаера
        fireButton.onClick.AddListener(ToggleFire);
        fireRemainingTime = fireTotalDuration; // Устанавливаем начальное время работы фаера
        UpdateFireUI();
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
        if (isRecharging || FireManager.Instance.GetTotalFireCount() <= 0) return; // Проверяем, не на перезарядке ли фаер и есть ли доступные фаеры

        isUsingFire = !isUsingFire;
        animator.SetBool("FireUse", isUsingFire);

        if (isUsingFire)
        {
            StartCoroutine(ActivateFireWithDelay());

            if (buttonToDisable1 != null) buttonToDisable1.interactable = false;
            if (buttonToDisable2 != null) buttonToDisable2.interactable = false;

            if (activationSoundObject != null)
            {
                activationSoundObject.SetActive(true);
                StartCoroutine(DisableSoundObjectAfterDelay(activationSoundObject, 1.5f));
            }

            if (!fireDepleting)
            {
                StartCoroutine(FireDepletionCoroutine());
            }
        }
        else
        {
            DeactivateFireEffects();

            if (buttonToDisable1 != null) buttonToDisable1.interactable = true;
            if (buttonToDisable2 != null) buttonToDisable2.interactable = true;

            if (deactivationSoundObject != null)
            {
                deactivationSoundObject.SetActive(true);
                StartCoroutine(DisableSoundObjectAfterDelay(deactivationSoundObject, 1.5f));
            }
        }
    }

    private IEnumerator FireDepletionCoroutine()
    {
        fireDepleting = true;

        // Здесь мы больше не сбрасываем fireRemainingTime, чтобы сохранить его текущее значение
        while (fireRemainingTime > 0 && isUsingFire)
        {
            fireRemainingTime -= Time.deltaTime;
            UpdateFireUI();
            yield return null;
        }

        if (fireRemainingTime <= 0)
        {
            isUsingFire = false;
            DeactivateFireEffects();
            animator.SetBool("FireUse", false);

            FireManager.Instance.DecreaseTotalFireCount(); // Уменьшаем общее количество фаеров
            fireRemainingTime = fireTotalDuration; // Сбрасываем оставшееся время на начальное значение для следующего фаера

            if (buttonToDisable1 != null) buttonToDisable1.interactable = true;
            if (buttonToDisable2 != null) buttonToDisable2.interactable = true;

            StartCoroutine(RechargeFireButton());
        }

        fireDepleting = false;
    }

    private void UpdateFireUI()
    {
        // Обновляем UI элементов в зависимости от оставшегося времени работы фаера
        float[] thresholds = { fireTotalDuration * 2 / 3, fireTotalDuration / 2, fireTotalDuration / 3 };

        for (int i = 0; i < fireUIElements.Length; i++)
        {
            if (fireUIElements[i] != null)
            {
                // Активируем/деактивируем элемент в зависимости от оставшегося времени работы фаера
                fireUIElements[i].SetActive(fireRemainingTime > thresholds[i]);
            }
        }

        // Также обновляем UI, если количество фаеров в FireManager равно 0
        if (FireManager.Instance.GetTotalFireCount() <= 0)
        {
            foreach (var element in fireUIElements)
            {
                if (element != null)
                {
                    element.SetActive(false); // Отключаем все UI элементы, если фаеров больше нет
                }
            }
        }
    }

    private void OnSceneLoaded()
    {
        // При загрузке новой сцены обновляем состояние UI, чтобы оно соответствовало текущему количеству фаеров
        UpdateFireUI();
    }


    private IEnumerator RechargeFireButton()
    {
        isRecharging = true; // Устанавливаем статус перезарядки
        fireButton.interactable = false; // Отключаем кнопку на время перезарядки

        yield return new WaitForSeconds(rechargeTime); // Ждем время перезарядки

        isRecharging = false; // Перезарядка завершена

        // Активируем кнопку, если еще остались фаеры
        if (totalFireCount > 0)
        {
            fireButton.interactable = true;
        }
    }

    private void DeactivateFireEffects()
    {
        // Отключаем все эффекты фаера
        if (fireVFX != null) fireVFX.SetActive(false);
        if (fireLight != null) fireLight.enabled = false;
        if (fire3DModel != null) fire3DModel.SetActive(false);

        // Останавливаем звук фаера
        if (fireLoopSoundObject != null) fireLoopSoundObject.SetActive(false);
    }



    private IEnumerator ActivateFireWithDelay()
    {
        yield return new WaitForSeconds(fireActivationDelay);
        // Включаем эффекты фаера после задержки
        if (fireVFX != null) fireVFX.SetActive(true);
        if (fireLight != null) fireLight.enabled = true;
        if (fire3DModel != null) fire3DModel.SetActive(true);

        // Включаем звук фаера
        if (fireLoopSoundObject != null) fireLoopSoundObject.SetActive(true);
    }

    private IEnumerator DisableSoundObjectAfterDelay(GameObject soundObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (soundObject != null) soundObject.SetActive(false);
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



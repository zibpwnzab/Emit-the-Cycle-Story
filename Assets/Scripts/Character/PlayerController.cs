using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    private const float SoundDisableDelay = 1.5f;
    private const float FallStunDuration = 5.0f;

    [Header("Components")]
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Animator animator;

    [Header("Movement Settings")]
    [SerializeField] private float runSpeedThreshold = 2f;
    [SerializeField] private float moveSpeed = 3f;
    public float movingObjectModifier = 0.5f;
    public float rotateObjectSpeed = 50f;
    [SerializeField] private float jumpforce = 5f;
    public float fallThreshold = 10f;

    [Header("Interaction & Health")]
    [SerializeField] private Button interactButton;
    public int Lifes = 3;

    [Header("Flare / Fire System")]
    [SerializeField] private GameObject fireVFX;
    [SerializeField] private Light fireLight;
    [SerializeField] private GameObject fire3DModel;
    [SerializeField] private Button fireButton;
    [SerializeField] private float fireActivationDelay = 1.5f;
    [SerializeField] private float fireTotalDuration = 6f;
    [SerializeField] private float fireRemainingTime;
    [SerializeField] private int totalFireCount = 3;
    [SerializeField] private float rechargeTime = 10f;
    [SerializeField] private GameObject[] fireUIElements;
    [SerializeField] private Button buttonToDisable1;
    [SerializeField] private Button buttonToDisable2;

    [Header("Flare Audio")]
    [SerializeField] private GameObject activationSoundObject;
    [SerializeField] private GameObject fireLoopSoundObject;
    [SerializeField] private GameObject deactivationSoundObject;

    [Header("Debug")]
    [SerializeField] private TMP_Text VelocityText;

    public PlayerState playerState = PlayerState.Walking;
    public static PlayerController instance;

    public static string PLAYER_CARMA_KEY = "PLAYER_CARMA_KEY";
    public static string NEXT_LEVEL_KEY = "NEXT_LEVEL_KEY";
    public static string TOTAL_GAME_TIME = "TOTAL_GAME_TIME";

    private bool _isUsingFire;
    private bool _fireDepleting;
    private bool _isRecharging;
    private bool _isJumping;
    private bool _isStunned;
    private float _defaultMoveSpeed;

    private readonly List<IInteractable> _interactables = new List<IInteractable>();
    private GameObject _lastInteractable;
    private Camera _mainCamera;

    private void Awake()
    {
        instance = this;
        _defaultMoveSpeed = moveSpeed;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (fireButton != null)
        {
            fireButton.onClick.RemoveListener(ToggleFire);
        }
    }

    private void Start()
    {
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
        if (animator == null) animator = GetComponent<Animator>();
        _mainCamera = Camera.main;

        if (fireButton != null)
        {
            fireButton.onClick.AddListener(ToggleFire);
        }

        fireRemainingTime = fireTotalDuration;
        UpdateFireUI();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (VelocityText != null && rigidbody != null)
        {
            VelocityText.text = rigidbody.linearVelocity.ToString("F2");
        }
#endif
        if (rigidbody == null) return;

        rigidbody.angularVelocity = Vector3.zero;
        CheckFallingState();

        if (animator != null && animator.GetBool("FireUse"))
        {
            bool isMoving = rigidbody.linearVelocity.magnitude > 0.1f;
            animator.SetBool("isRunning", isMoving);
        }
    }

    private void FixedUpdate()
    {
        if (_isStunned || rigidbody == null) return;

        switch (playerState)
        {
            case PlayerState.Walking:
                MoveStandard();
                break;
            case PlayerState.MovingObject:
                MoveWithObject();
                break;
            case PlayerState.RotatingObject:
                RotateObject();
                break;
        }
    }

    private void ToggleFire()
    {
        int availableFlares = FireManager.Instance != null
            ? FireManager.Instance.GetTotalFireCount()
            : totalFireCount;

        if (_isRecharging || availableFlares <= 0) return;

        _isUsingFire = !_isUsingFire;
        if (animator != null) animator.SetBool("FireUse", _isUsingFire);

        if (_isUsingFire)
        {
            StartCoroutine(ActivateFireWithDelay());
            SetSecondaryButtonsInteractable(false);
            TriggerSoundObject(activationSoundObject, SoundDisableDelay);

            if (!_fireDepleting)
            {
                StartCoroutine(FireDepletionRoutine());
            }
        }
        else
        {
            DeactivateFireEffects();
            SetSecondaryButtonsInteractable(true);
            TriggerSoundObject(deactivationSoundObject, SoundDisableDelay);
        }
    }

    private IEnumerator FireDepletionRoutine()
    {
        _fireDepleting = true;

        while (fireRemainingTime > 0f && _isUsingFire)
        {
            fireRemainingTime -= Time.deltaTime;
            UpdateFireUI();
            yield return null;
        }

        if (fireRemainingTime <= 0f)
        {
            _isUsingFire = false;
            DeactivateFireEffects();
            if (animator != null) animator.SetBool("FireUse", false);

            if (FireManager.Instance != null)
            {
                FireManager.Instance.DecreaseTotalFireCount();
            }
            else
            {
                totalFireCount = Mathf.Max(0, totalFireCount - 1);
            }

            fireRemainingTime = fireTotalDuration;
            SetSecondaryButtonsInteractable(true);
            StartCoroutine(RechargeFireButtonRoutine());
        }

        _fireDepleting = false;
    }

    private void UpdateFireUI()
    {
        if (fireUIElements == null || fireUIElements.Length == 0) return;

        int count = FireManager.Instance != null
            ? FireManager.Instance.GetTotalFireCount()
            : totalFireCount;

        if (count <= 0)
        {
            foreach (var element in fireUIElements)
            {
                if (element != null) element.SetActive(false);
            }
            return;
        }

        float[] thresholds =
        {
            fireTotalDuration * 2f / 3f,
            fireTotalDuration * 0.5f,
            fireTotalDuration / 3f
        };

        for (int i = 0; i < fireUIElements.Length && i < thresholds.Length; i++)
        {
            if (fireUIElements[i] != null)
            {
                fireUIElements[i].SetActive(fireRemainingTime > thresholds[i]);
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _mainCamera = Camera.main;
        UpdateFireUI();
    }

    private IEnumerator RechargeFireButtonRoutine()
    {
        _isRecharging = true;
        if (fireButton != null) fireButton.interactable = false;

        yield return new WaitForSeconds(rechargeTime);

        _isRecharging = false;
        int remaining = FireManager.Instance != null
            ? FireManager.Instance.GetTotalFireCount()
            : totalFireCount;

        if (remaining > 0 && fireButton != null)
        {
            fireButton.interactable = true;
        }
    }

    private void DeactivateFireEffects()
    {
        if (fireVFX != null) fireVFX.SetActive(false);
        if (fireLight != null) fireLight.enabled = false;
        if (fire3DModel != null) fire3DModel.SetActive(false);
        if (fireLoopSoundObject != null) fireLoopSoundObject.SetActive(false);
    }

    private IEnumerator ActivateFireWithDelay()
    {
        yield return new WaitForSeconds(fireActivationDelay);
        if (!_isUsingFire) yield break;

        if (fireVFX != null) fireVFX.SetActive(true);
        if (fireLight != null) fireLight.enabled = true;
        if (fire3DModel != null) fire3DModel.SetActive(true);
        if (fireLoopSoundObject != null) fireLoopSoundObject.SetActive(true);
    }

    private void TriggerSoundObject(GameObject soundObject, float duration)
    {
        if (soundObject == null) return;
        soundObject.SetActive(true);
        StartCoroutine(DisableObjectAfterDelay(soundObject, duration));
    }

    private IEnumerator DisableObjectAfterDelay(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (target != null) target.SetActive(false);
    }

    private void SetSecondaryButtonsInteractable(bool state)
    {
        if (buttonToDisable1 != null) buttonToDisable1.interactable = state;
        if (buttonToDisable2 != null) buttonToDisable2.interactable = state;
    }

    public void ForceKick(Vector3 direction, float stunTime)
    {
        if (_lastInteractable != null && _lastInteractable.TryGetComponent(out IInteractable interactable))
        {
            interactable.StopInteraction(gameObject, animator);
        }

        if (rigidbody != null)
        {
            rigidbody.linearVelocity = direction;
        }

        if (!_isStunned)
        {
            StartCoroutine(StunRecoverRoutine(stunTime));
        }
    }

    public void TakeDamage(int damage)
    {
        Lifes -= damage;
        if (Lifes <= 0)
        {
            Die();
        }
    }

    private IEnumerator StunRecoverRoutine(float stunDuration)
    {
        _isStunned = true;
        playerState = PlayerState.Stunned;
        if (animator != null) animator.SetBool("Stuned", true);

        yield return new WaitForSeconds(stunDuration);

        _isStunned = false;
        playerState = PlayerState.Walking;
        moveSpeed = _defaultMoveSpeed;
        if (animator != null) animator.SetBool("Stuned", false);
    }

    private void RotateObject()
    {
        if (joystick == null || _lastInteractable == null) return;

        if (Mathf.Abs(joystick.Horizontal) > Mathf.Abs(joystick.Vertical))
        {
            _lastInteractable.transform.Rotate(Vector3.up, Time.deltaTime * joystick.Horizontal * rotateObjectSpeed);
        }
    }

    private void MoveStandard()
    {
        if (joystick == null) return;

        Vector3 inputVelocity = new Vector3(
            joystick.Horizontal * moveSpeed,
            rigidbody.linearVelocity.y,
            joystick.Vertical * moveSpeed);

        Vector3 desiredVelocity = TransformInputRelativeToCamera(inputVelocity);
        rigidbody.linearVelocity = desiredVelocity;

        if (joystick.Horizontal != 0f || joystick.Vertical != 0f)
        {
            Vector3 planarVelocity = new Vector3(rigidbody.linearVelocity.x, 0f, rigidbody.linearVelocity.z);
            if (planarVelocity.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(planarVelocity);
            }

            if (!_isJumping && animator != null)
            {
                animator.SetFloat("Speed", Vector3.ClampMagnitude(planarVelocity, 1f).magnitude);
                animator.SetBool("isRunning", planarVelocity.magnitude > runSpeedThreshold);
            }
        }
        else if (!_isJumping && animator != null)
        {
            animator.SetFloat("Speed", 0f);
            animator.SetBool("isRunning", false);
        }
    }

    private void MoveWithObject()
    {
        if (joystick == null) return;

        float modifiedSpeed = moveSpeed * movingObjectModifier;
        Vector3 inputVelocity = new Vector3(
            joystick.Horizontal * modifiedSpeed,
            rigidbody.linearVelocity.y,
            joystick.Vertical * modifiedSpeed);

        rigidbody.linearVelocity = TransformInputRelativeToCamera(inputVelocity);

        bool isMoving = joystick.Horizontal != 0f || joystick.Vertical != 0f;
        if (animator != null)
        {
            animator.SetBool("Pushing", playerState == PlayerState.MovingObject && isMoving);
        }
    }

    private Vector3 TransformInputRelativeToCamera(Vector3 direction)
    {
        if (_mainCamera == null) _mainCamera = Camera.main;
        if (_mainCamera == null) return direction;

        Vector3 forward = _mainCamera.transform.forward;
        Vector3 right = _mainCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * direction.z + right * direction.x;
        desiredMoveDirection.y = direction.y;
        return desiredMoveDirection;
    }

    public IEnumerator Jump()
    {
        if (!_isJumping && rigidbody != null)
        {
            if (animator != null) animator.SetTrigger("Jumping");
            rigidbody.linearVelocity = new Vector3(
                rigidbody.linearVelocity.x,
                jumpforce,
                rigidbody.linearVelocity.z);
            _isJumping = true;
        }
        yield return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isJumping = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IInteractable obj)) return;
        if (_interactables.Contains(obj)) return;

        if (_lastInteractable == null)
        {
            _lastInteractable = other.gameObject;
        }

        _interactables.Add(obj);
        ResolveInteraction();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out IInteractable obj)) return;
        if (!_interactables.Contains(obj)) return;

        if (other.gameObject == _lastInteractable)
        {
            _lastInteractable = null;
        }

        _interactables.Remove(obj);
        ResolveInteraction();
    }

    private void ResolveInteraction()
    {
        if (interactButton == null) return;

        interactButton.onClick.RemoveAllListeners();
        if (_interactables.Count > 0)
        {
            interactButton.onClick.AddListener(() => _interactables[0].Interact(gameObject, animator));
        }
    }

    public void Die()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.FinishLevel(false);
        }
    }

    private void CheckFallingState()
    {
        if (!_isStunned && rigidbody.linearVelocity.y < -fallThreshold)
        {
            StartCoroutine(StunRecoverRoutine(FallStunDuration));
        }
    }
}

public enum PlayerState
{
    Walking,
    Dead,
    MovingObject,
    RotatingObject,
    Stunned
}
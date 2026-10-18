using UnityEngine;

public class LaserReceiver : ISignal
{
    private const float PowerGracePeriod = 0.15f;

    public bool powered;

    [SerializeField] private MeshRenderer meshToLight;
    [SerializeField] private Animator animator;
    [SerializeField] private Material onMaterial;
    [SerializeField] private Material offMaterial;

    private float _lastPoweredTime = -1f;
    private bool _lastVisualState;
    private bool _isVisualInitialized;

    public override bool Signal()
    {
        // Wider window at low FPS so the signal does not flicker
        float gracePeriod = Mathf.Max(PowerGracePeriod, Time.deltaTime * 2f);
        return Time.time - _lastPoweredTime <= gracePeriod;
    }

    public void Power(bool isPowered)
    {
        if (isPowered)
        {
            _lastPoweredTime = Time.time;
        }
    }

    private void Update()
    {
        bool isActive = Signal();
        powered = isActive;

        if (animator != null)
        {
            animator.SetFloat("speed", isActive ? 1f : 0f);
        }

        if (meshToLight != null && (!_isVisualInitialized || _lastVisualState != isActive))
        {
            _isVisualInitialized = true;
            _lastVisualState = isActive;
            meshToLight.sharedMaterial = isActive ? onMaterial : offMaterial;
        }
    }
}
using UnityEngine;

public class LedgeGrab : MonoBehaviour
{
    private const float SnapDistance = 0.1f;

    public float pullSpeed = 5f;
    public Transform ledgePosition;

    private Rigidbody _targetRb;
    private bool _isPulling;

    private void Start()
    {
        if (TryGetComponent(out Rigidbody selfRb))
        {
            selfRb.isKinematic = true;
            _targetRb = selfRb;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_targetRb == null)
            {
                _targetRb = other.GetComponent<Rigidbody>();
            }
            _isPulling = true;
        }
    }

    private void Update()
    {
        if (!_isPulling || ledgePosition == null || _targetRb == null) return;

        Vector3 currentPos = _targetRb.position;
        Vector3 direction = (ledgePosition.position - currentPos).normalized;
        float distance = Vector3.Distance(currentPos, ledgePosition.position);

        _targetRb.MovePosition(currentPos + direction * (pullSpeed * Time.deltaTime));

        if (distance < SnapDistance)
        {
            _isPulling = false;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(LineRenderer))]
public class LaserEmitter : MonoBehaviour
{
    private const float MaxRayDistance = 100f;
    private const float SurfaceOffset = 0.001f;
    private const int BounceLimit = 32;

    [Header("Damage & Knockback")]
    // Old field names are still used in the turret prefabs
    [FormerlySerializedAs("LaserKickForce")]
    [SerializeField] private float laserKickForce = 10f;
    [FormerlySerializedAs("LaserStunTime")]
    [SerializeField] private float laserStunTime = 3f;

    [Header("Ray Settings")]
    [SerializeField] private Transform laserDirection;
    [SerializeField] private Transform laserStart;
    [SerializeField] private int maxBounces = 3;
    [SerializeField] private int mirrorLayer;
    [SerializeField] private LayerMask collideLayer;
    [SerializeField] private int positionsPerSegment = 10;

    [Header("Signal Dependency")]
    [FormerlySerializedAs("NeedSignal")]
    [SerializeField] private bool needSignal;
    [SerializeField] private ISignal signalSource;

    private LineRenderer _lineRenderer;
    private readonly List<Vector3> _laserPositions = new List<Vector3>();
    private Vector3[] _linePoints = new Vector3[0];
    // Timestamp instead of a coroutine, which stops if the emitter gets disabled mid-cooldown
    private float _nextDamageTime;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnValidate()
    {
        maxBounces = Mathf.Clamp(maxBounces, 0, BounceLimit);
    }

    private void Update()
    {
        if (laserStart == null || laserDirection == null) return;

        _laserPositions.Clear();
        _laserPositions.Add(laserStart.position);

        bool shouldEmit = !needSignal || (signalSource != null && signalSource.Evaluate());
        if (shouldEmit)
        {
            CastLaserBeam(laserStart.position, laserDirection.forward.normalized, 0);
        }

        RenderLaserLine();
    }

    private void CastLaserBeam(Vector3 origin, Vector3 direction, int bounceIndex)
    {
        if (bounceIndex > Mathf.Min(maxBounces, BounceLimit)) return;

        Ray ray = new Ray(origin, direction);
        if (!Physics.Raycast(ray, out RaycastHit hit, MaxRayDistance, collideLayer))
        {
            _laserPositions.Add(origin + direction * MaxRayDistance);
            return;
        }

        _laserPositions.Add(hit.point);
        GameObject hitObject = hit.collider.gameObject;

        if (hitObject.layer == mirrorLayer)
        {
            Vector3 reflectedDir = Vector3.Reflect(direction, hit.normal).normalized;
            CastLaserBeam(hit.point + reflectedDir * SurfaceOffset, reflectedDir, bounceIndex + 1);
        }

        if (hitObject.TryGetComponent(out LaserReceiver receiver))
        {
            receiver.Power(true);
        }

        if (Time.time >= _nextDamageTime && hitObject.TryGetComponent(out PlayerController player))
        {
            Vector3 knockbackDir = player.transform.position - hit.point;
            knockbackDir.y = 0f;
            knockbackDir = knockbackDir.normalized * laserKickForce;

            _nextDamageTime = Time.time + laserStunTime;
            player.ForceKick(knockbackDir, laserStunTime);
            player.TakeDamage(1);
        }

        if (hitObject.TryGetComponent(out DestroyableObject destroyable))
        {
            destroyable.TakeDamage(Time.deltaTime);
        }
    }

    private void RenderLaserLine()
    {
        if (_laserPositions.Count < 2)
        {
            _lineRenderer.positionCount = 0;
            return;
        }

        int segmentSubdivisions = Mathf.Max(1, positionsPerSegment);
        int pointCount = (_laserPositions.Count - 1) * segmentSubdivisions + 1;
        // Reallocated only when the number of bounces changes.
        if (_linePoints.Length != pointCount)
        {
            _linePoints = new Vector3[pointCount];
        }

        for (int i = 0; i < _laserPositions.Count - 1; i++)
        {
            Vector3 segmentVector = _laserPositions[i + 1] - _laserPositions[i];
            for (int j = 0; j < segmentSubdivisions; j++)
            {
                _linePoints[i * segmentSubdivisions + j] =
                    _laserPositions[i] + segmentVector * ((float)j / segmentSubdivisions);
            }
        }

        _linePoints[pointCount - 1] = _laserPositions[_laserPositions.Count - 1];

        _lineRenderer.positionCount = pointCount;
        _lineRenderer.SetPositions(_linePoints);
    }
}

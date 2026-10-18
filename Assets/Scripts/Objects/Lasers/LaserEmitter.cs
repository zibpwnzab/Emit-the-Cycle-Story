using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(LineRenderer))]
public class LaserEmitter : MonoBehaviour
{
    private const float MaxRayDistance = 100f;
    private const float SurfaceOffset = 0.001f;

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
    private bool _canDealDamage = true;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (laserStart == null || laserDirection == null) return;

        _laserPositions.Clear();
        _laserPositions.Add(laserStart.position);

        bool shouldEmit = !needSignal || (signalSource != null && signalSource.Signal());
        if (shouldEmit)
        {
            CastLaserBeam(laserStart.position, laserDirection.forward.normalized, 0);
        }

        RenderLaserLine();
    }

    private void CastLaserBeam(Vector3 origin, Vector3 direction, int bounceIndex)
    {
        if (bounceIndex > maxBounces) return;

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

        if (hitObject.TryGetComponent(out PlayerController player) && _canDealDamage)
        {
            Vector3 knockbackDir = player.transform.position - hit.point;
            knockbackDir.y = 0f;
            knockbackDir = knockbackDir.normalized * laserKickForce;

            player.ForceKick(knockbackDir, laserStunTime);
            player.TakeDamage(1);
            StartCoroutine(DamageCooldownRoutine());
        }

        if (hitObject.TryGetComponent(out DestroyableObject destroyable))
        {
            destroyable.TakeDamage(Time.deltaTime);
        }
    }

    private IEnumerator DamageCooldownRoutine()
    {
        _canDealDamage = false;
        yield return new WaitForSeconds(laserStunTime);
        _canDealDamage = true;
    }

    private void RenderLaserLine()
    {
        if (_laserPositions.Count < 2)
        {
            _lineRenderer.positionCount = 0;
            return;
        }

        int segmentSubdivisions = Mathf.Max(1, positionsPerSegment);
        _lineRenderer.positionCount = (_laserPositions.Count - 1) * segmentSubdivisions + 1;

        for (int i = 0; i < _laserPositions.Count - 1; i++)
        {
            Vector3 segmentVector = _laserPositions[i + 1] - _laserPositions[i];
            for (int j = 0; j < segmentSubdivisions; j++)
            {
                Vector3 interpolatedPos = _laserPositions[i] + segmentVector * ((float)j / segmentSubdivisions);
                _lineRenderer.SetPosition(i * segmentSubdivisions + j, interpolatedPos);
            }
        }

        _lineRenderer.SetPosition(
            (_laserPositions.Count - 1) * segmentSubdivisions,
            _laserPositions[_laserPositions.Count - 1]);
    }
}
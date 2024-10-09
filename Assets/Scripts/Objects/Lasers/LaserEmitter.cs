using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserEmitter : MonoBehaviour
{
    [SerializeField] private float laserKickForce = 10f;
    [SerializeField] private float laserStunTime = 3f;

    private LineRenderer _lineRenderer;
    [SerializeField] private Transform laserDirection;
    [SerializeField] private Transform laserStart;
    [SerializeField] private int maxBounces = 3;
    private List<Vector3> laserPositions;
    [SerializeField] private int mirrorLayer;
    [SerializeField] private LayerMask collideLayer;
    [SerializeField] private int positionsPerSegment = 10;
    [SerializeField] private bool needSignal;
    [SerializeField] private ISignal signalSource;

    private bool canDealDamage = true; // Флаг для управления нанесением урона

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        laserPositions = new List<Vector3>();
        laserPositions.Add(laserStart.position);

        if (needSignal)
        {
            if (signalSource.Signal())
                EmitLaser(laserStart.position, laserDirection.forward, 0);
        }
        else
        {
            EmitLaser(laserStart.position, laserDirection.forward, 0);
        }

        DrawLaser();
    }

    void EmitLaser(Vector3 position, Vector3 direction, int currentCount)
    {
        if (currentCount > maxBounces)
            return;

        Ray ray = new Ray(position, direction);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, float.MaxValue, collideLayer))
        {
            laserPositions.Add(position + direction * 100);
            return;
        }

        laserPositions.Add(hit.point);

        if (hit.collider.gameObject.layer == mirrorLayer)
        {
            Vector3 inRay = hit.point - position;
            Vector3 outRay = inRay - 2 * Vector3.Dot(inRay, hit.normal.normalized) * hit.normal.normalized;
            EmitLaser(hit.point, outRay, currentCount + 1);
        }

        if (hit.collider.gameObject.TryGetComponent(out LaserReceiver laserReceiver))
        {
            laserReceiver.Power(true);
        }

        if (hit.collider.gameObject.TryGetComponent(out PlayerController player))
        {
            if (canDealDamage) // Проверяем, можно ли наносить урон
            {
                Vector3 dir = player.transform.position - hit.point;
                dir.y = 0;
                dir = dir.normalized * laserKickForce;

                player.ForceKick(dir, laserStunTime);
                player.TakeDamage(1);

                StartCoroutine(DisableLaserDamageTemporarily()); // Запускаем корутину для временного отключения урона
            }
        }

        if (hit.collider.gameObject.TryGetComponent(out DestroyableObject destroyableObject))
        {
            destroyableObject.TakeDamage(1 * Time.deltaTime);
        }
    }

    /// <summary>
    /// Корутин для временного отключения урона лазера.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisableLaserDamageTemporarily()
    {
        canDealDamage = false; // Отключаем урон
        yield return new WaitForSeconds(laserStunTime); // Ждем 3 секунды
        canDealDamage = true; // Включаем урон обратно
    }

    void DrawLaser()
    {
        var previousPos = laserPositions[0];

        _lineRenderer.positionCount = (laserPositions.Count - 1) * positionsPerSegment + 1;

        for (int i = 0; i < laserPositions.Count - 1; i++)
        {
            var dir = laserPositions[i + 1] - laserPositions[i];
            for (int j = 0; j < positionsPerSegment; j++)
            {
                var pos = laserPositions[i] + dir * ((float)j / (float)positionsPerSegment);
                _lineRenderer.SetPosition(i * positionsPerSegment + j, pos);
            }
        }
        _lineRenderer.SetPosition((laserPositions.Count - 1) * positionsPerSegment, laserPositions[laserPositions.Count - 1]);
    }
}

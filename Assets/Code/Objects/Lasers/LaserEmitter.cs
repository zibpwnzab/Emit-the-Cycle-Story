using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserEmitter : MonoBehaviour
{
    LineRenderer _lineRenderer;
    [SerializeField] Transform laserDirection;
    [SerializeField] Transform laserStart;
    [SerializeField] int maxBounces;
    List<Vector3> laserPositions;
    [SerializeField] int mirrorLayer;
    [SerializeField] LayerMask collideLayer;
    [SerializeField] int positionsPerSegment;
    [SerializeField] bool NeedSignal;
    [SerializeField] ISignal signalSource;
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        laserPositions = new();
        laserPositions.Add(laserStart.position);
        if (NeedSignal)
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
        Ray ray;
        RaycastHit hit;

        if (!Physics.Raycast(position, direction.normalized, out hit, float.MaxValue, collideLayer))
        {
            laserPositions.Add(position + direction * 100);
            return;
        }

        laserPositions.Add(hit.point);
        if (hit.collider.gameObject.layer == mirrorLayer)
        {
            var in_ray = hit.point - position;
            var out_ray = in_ray - 2 * Vector3.Dot(in_ray, hit.normal.normalized) * hit.normal.normalized;
            Debug.Log("Mirror Hit");
            EmitLaser(hit.point, out_ray, currentCount + 1);
        }
        


        if (hit.collider.gameObject.TryGetComponent(out LaserReceiver laserReceiver))
        {
            laserReceiver.Power(true);
        }
    }

    void MirrorLaser()
    {

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
                var pos = laserPositions[i] + dir * ((float)j/(float)positionsPerSegment);
                _lineRenderer.SetPosition(i * positionsPerSegment + j, pos);
            }
        }
        _lineRenderer.SetPosition((laserPositions.Count - 1) * positionsPerSegment, laserPositions[laserPositions.Count-1]);
    }
}

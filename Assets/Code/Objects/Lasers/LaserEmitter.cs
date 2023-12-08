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
    [SerializeField] GameObject signalSource;
    ISignal signal;
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        if (signalSource != null)
            signal = signalSource.GetComponent<ISignal>();
    }

    // Update is called once per frame
    void Update()
    {
        laserPositions = new();
        laserPositions.Add(laserStart.position);
        if (signal != null)
        {
            if (signal.Signal())
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

        if (!Physics.Raycast(position, direction.normalized, out hit))
            return;
        laserPositions.Add(hit.point);
        if (hit.collider.gameObject.layer == mirrorLayer)
        {
            var in_ray = hit.point - position;
            var out_ray = in_ray - 2 * Vector3.Dot(in_ray, hit.normal.normalized) * hit.normal.normalized;
            Debug.Log("Mirror Hit");
            EmitLaser(hit.point, out_ray, currentCount + 1);
        }

        LaserReceiver laserReceiver;
        if (hit.collider.gameObject.TryGetComponent(out laserReceiver))
        {
            laserReceiver.Power(true);
        }
    }

    void MirrorLaser()
    {

    }

    void DrawLaser()
    {
        _lineRenderer.positionCount = laserPositions.Count;
        for (int i = 0; i < laserPositions.Count; i++)
        {
            _lineRenderer.SetPosition(i, laserPositions[i]);
        }
    }
}

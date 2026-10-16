using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour
{
    [SerializeField] private Transform ropeEnd;
    [SerializeField] private Transform ropeStart;
    [SerializeField] private float magnitudeError = 0.5f;
    [SerializeField] private float angularLimit = 5f;

    private LineRenderer _lineRenderer;
    private List<RopeSegment> _segments;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _segments = new List<RopeSegment>(GetComponentsInChildren<RopeSegment>());
        _lineRenderer.positionCount = _segments.Count + 2;
    }

    private void Update()
    {
        if (ropeStart == null || ropeEnd == null || _segments == null || _segments.Count == 0)
        {
            return;
        }

        UpdatePositions();
        DrawRope();
    }

    private void UpdatePositions()
    {
        Vector3 previousPos = ropeStart.position;
        float sqrErrorLimit = magnitudeError * magnitudeError;

        foreach (RopeSegment segment in _segments)
        {
            if (segment == null) continue;

            Vector3 dir = previousPos - segment.transform.position;
            if (dir.sqrMagnitude >= sqrErrorLimit)
            {
                segment.transform.position = (previousPos + 3f * segment.transform.position) * 0.25f;
            }

            if (segment.EndPoint != null)
            {
                previousPos = segment.EndPoint.position;
            }
        }

        if ((ropeStart.position - _segments[0].transform.position).sqrMagnitude > sqrErrorLimit)
        {
            _segments[0].transform.position = ropeStart.position;
        }

        foreach (RopeSegment segment in _segments)
        {
            Rigidbody rb = segment != null ? segment.CachedRigidbody : null;
            if (rb != null)
            {
                rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, angularLimit);
                rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, angularLimit);
            }
        }

        RopeSegment lastSegment = _segments[_segments.Count - 1];
        if (lastSegment != null && lastSegment.EndPoint != null)
        {
            if ((ropeEnd.position - lastSegment.EndPoint.position).sqrMagnitude > sqrErrorLimit)
            {
                ropeEnd.position = lastSegment.EndPoint.position;
            }
        }
    }

    private void DrawRope()
    {
        _lineRenderer.SetPosition(0, ropeStart.position);

        for (int i = 0; i < _segments.Count; i++)
        {
            if (_segments[i] != null)
            {
                _lineRenderer.SetPosition(i + 1, _segments[i].transform.position);
            }
        }

        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, ropeEnd.position);
    }
}
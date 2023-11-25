using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] Transform ropeEnd;
    [SerializeField] Transform ropeStart;
    [SerializeField] float magnitudeError;
    [SerializeField] float angularLimit;
    LineRenderer _lineRenderer;

    List<RopeSegment> _childs;
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        _childs = new(GetComponentsInChildren<RopeSegment>());
        _lineRenderer.positionCount = _childs.Count + 2;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        DrawRope();
        
    }

    void UpdatePosition()
    {
        var previous_pos = ropeStart.position;
        float total_length = 0;
        foreach (var rope in _childs)
        {
            var dir = previous_pos - rope.transform.position;
            total_length += dir.magnitude;
            if (dir.sqrMagnitude >= magnitudeError * magnitudeError)
            {
                rope.transform.position = (previous_pos + 3 * rope.transform.position) / 4;
            }
            previous_pos = rope.EndPoint.position;
        }
        if ((ropeStart.position - _childs[0].transform.position).sqrMagnitude > magnitudeError * magnitudeError)
            _childs[0].transform.position = ropeStart.position;
/*
        for (int i = 1; i < _childs.Count; i++)
        {
            if ((_childs[i].transform.position - _childs[i - 1].EndPoint.position).sqrMagnitude < magnitudeError * magnitudeError)
                continue;
                _childs[i].transform.position = _childs[i - 1].EndPoint.position;
        }*/

        foreach (var c in _childs)
        {
            c.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.ClampMagnitude(c.gameObject.GetComponent<Rigidbody>().angularVelocity, angularLimit);
            c.gameObject.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(c.gameObject.GetComponent<Rigidbody>().velocity, angularLimit);
        }
        if ((ropeEnd.position - _childs[_childs.Count - 1].EndPoint.position).sqrMagnitude > magnitudeError * magnitudeError)
            ropeEnd.position = _childs[_childs.Count - 1].EndPoint.position;
        
    }

    void DrawRope()
    {
        _lineRenderer.SetPosition(0, ropeStart.position);
        for (int i = 1; i <= _childs.Count; i++)
        {
            _lineRenderer.SetPosition(i, _childs[i - 1].transform.position);

        }
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, ropeEnd.position);
    }
}



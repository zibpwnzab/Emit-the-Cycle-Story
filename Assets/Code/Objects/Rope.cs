using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] Transform ropeEnd;
    [SerializeField] Transform ropeStart;
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
        _childs[0].transform.position = ropeStart.position;
        for (int i = 1; i < _childs.Count; i++)
        {
            if ((_childs[i].transform.position - _childs[i - 1].EndPoint.position).sqrMagnitude > 0.1)
                _childs[i].transform.position = _childs[i - 1].EndPoint.position;
        }
        ropeEnd.position = _childs[_childs.Count - 1].EndPoint.position;
        _lineRenderer.SetPosition(0, ropeStart.position);

        for (int i = 1; i <= _childs.Count; i++)
        {
            _lineRenderer.SetPosition(i, _childs[i - 1].transform.position);

        }
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1 , ropeEnd.position);
    }
}



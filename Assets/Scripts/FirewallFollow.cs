using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallFollow : MonoBehaviour
{
    public Transform Target;
    public float Speed;
    public float RelaxDistance;

    void Update()
    {
        var dir = Target.position - transform.position;
        if (dir.sqrMagnitude > RelaxDistance * RelaxDistance)
        {
            float step = Speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, Target.position, step);

        }
    }
}

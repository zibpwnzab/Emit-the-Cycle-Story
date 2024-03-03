using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchButton : ISignal, IInteractable
{
    private bool power;
    private Collider triger;
    public bool Interact(GameObject gameObject, Animator animator)
    {
        power = true;
        return power;
    }

    public override bool Signal()
    {
        return power;
    }

    public bool StopInteraction(GameObject gameObject, Animator animator)
    {
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var collider in GetComponents<Collider>())
        {
            if (!collider.isTrigger)
                continue;
            triger = collider;
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        triger.enabled = !power;
    }
}

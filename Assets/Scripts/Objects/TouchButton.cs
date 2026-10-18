using UnityEngine;

public class TouchButton : ISignal, IInteractable
{
    [SerializeField] bool onCollider;
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

    void Start()
    {
        foreach (var collider in GetComponents<Collider>())
        {
            if (!collider.isTrigger)
                continue;
            triger = collider;
            break;
        }

        if (triger == null)
        {
            Debug.LogWarning($"TouchButton '{name}' has no trigger collider.", this);
        }
    }

    void Update()
    {
        if (triger != null)
        {
            triger.enabled = !power || onCollider;
        }
    }
}

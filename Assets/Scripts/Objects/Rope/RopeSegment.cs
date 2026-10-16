using UnityEngine;

public class RopeSegment : MonoBehaviour
{
    public Transform EndPoint;
    public Rigidbody CachedRigidbody { get; private set; }

    private void Awake()
    {
        CachedRigidbody = GetComponent<Rigidbody>();
    }
}
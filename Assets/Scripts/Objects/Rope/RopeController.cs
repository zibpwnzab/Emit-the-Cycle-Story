using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RopeController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 input = Vector3.forward * Input.GetAxis("Vertical") +
                        Vector3.right * Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift)) input += Vector3.up;
        if (Input.GetKey(KeyCode.LeftControl)) input += Vector3.down;

        _rigidbody.linearVelocity = Vector3.ClampMagnitude(
            input * speed + _rigidbody.linearVelocity,
            speed);
    }
}
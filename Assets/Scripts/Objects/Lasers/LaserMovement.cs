using UnityEngine;

public class LaserMovement : MonoBehaviour
{
    public float speed = 5f;
    public float maxHeight = 10f;
    public float minHeight = 0f;

    private bool _movingUp = true;

    private void Update()
    {
        if (transform.position.y >= maxHeight)
        {
            _movingUp = false;
        }
        else if (transform.position.y <= minHeight)
        {
            _movingUp = true;
        }

        Vector3 direction = _movingUp ? Vector3.up : Vector3.down;
        transform.Translate(direction * (speed * Time.deltaTime));
    }
}
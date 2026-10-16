using System.Collections;
using UnityEngine;

public class ParabolaMovement : MonoBehaviour
{
    public float jumpDuration = 1f;
    public float jumpHeight = 2f;
    public float forwardDistance = 10f;

    [SerializeField] private Rigidbody rb;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    public IEnumerator Jump()
    {
        if (rb == null || jumpDuration <= 0f) yield break;

        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            float height = 4f * jumpHeight * t * (1f - t);
            float forward = Mathf.Lerp(0f, forwardDistance, t);

            Vector3 newPosition = new Vector3(
                startPosition.x + forward,
                startPosition.y + height,
                startPosition.z);

            rb.MovePosition(newPosition);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Vector3 finalPosition = new Vector3(
            startPosition.x + forwardDistance,
            startPosition.y,
            startPosition.z);

        rb.MovePosition(finalPosition);
    }
}
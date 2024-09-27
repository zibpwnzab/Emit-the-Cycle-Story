using System.Collections;
using UnityEngine;

public class ParabolaMovement : MonoBehaviour
{
    public float jumpDuration = 1f; 
    public float jumpHeight = 2f;  
    public float forwardDistance = 10f;  

    private Vector3 startPosition;
    [SerializeField] private Rigidbody rb;

    public IEnumerator Jump()
    {
        startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;

            float height = 4 * jumpHeight * t * (1 - t);

            float forward = Mathf.Lerp(0, forwardDistance, t);

            Vector3 newPosition = new Vector3(startPosition.x + forward, startPosition.y + height, startPosition.z);
            rb.MovePosition(newPosition);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Vector3 finalPosition = new Vector3(startPosition.x + forwardDistance, startPosition.y, startPosition.z);
        rb.MovePosition(finalPosition);
    }
}

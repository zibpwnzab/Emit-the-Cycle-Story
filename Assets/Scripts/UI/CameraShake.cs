using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform cameraTransform; 
    public float shakeDuration = 0.5f; 
    public float shakeAmount = 0.7f;
    private bool enter;

    private Vector3 originalPosition; 
    private float shakeTimer = 5f; 

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ShakeCamera();
    }

    public void ShakeCamera()
    {
            originalPosition = cameraTransform.localPosition;
            shakeTimer = shakeDuration;


            StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        while (shakeTimer > 0)
        {
            
            Vector2 shakeOffset = Random.insideUnitCircle * shakeAmount;
            Vector3 newPosition = originalPosition + new Vector3(shakeOffset.x, shakeOffset.y, 0f);

            
            cameraTransform.localPosition = newPosition;

            
            shakeTimer -= Time.deltaTime;

            yield return null;
        }

        
        cameraTransform.localPosition = originalPosition;
    }
}

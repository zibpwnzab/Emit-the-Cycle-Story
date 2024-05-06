using UnityEngine;

public class LedgeGrab : MonoBehaviour
{
    public float pullSpeed = 5f; 
    public Transform ledgePosition; 

    private Rigidbody rb;
    private bool isPulling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            isPulling = true;
            Debug.Log("GGG");
        }
        
    }

    void Update()
    {
        if (isPulling)
        {
            
            Vector3 direction = (ledgePosition.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, ledgePosition.position);

            rb.MovePosition(rb.position + direction * pullSpeed * Time.deltaTime);

            if (distance < 0.1f)
            {
                isPulling = false;
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] public LaserReceiver signal;
    [SerializeField] public GameObject door;
    [SerializeField] public GameObject[] plane; 
    float y;
    void Update()
    {
        
            if (signal.powered == true && y < 10)
            {
            foreach (GameObject obj in plane)
            {
                Renderer plane = obj.GetComponent<Renderer>();
                plane.material.color = new Color(37, 232, 8);
            }
                y += 0.05f;
                transform.position = new Vector2(77.84245f, y);

            }
        
    }
}

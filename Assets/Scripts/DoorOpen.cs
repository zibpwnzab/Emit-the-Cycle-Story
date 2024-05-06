using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public LaserReceiver signal;
    public GameObject door;
    float y; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (signal.powered == true)
        {
            y += 0.05f;
            transform.position = new Vector2 (77.84245f,y);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2point5Manager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] float cameraMovementSpeed;
    [SerializeField] bool linearMovement;
    [SerializeField] PlayerController player;

    Rigidbody _cameraRigidbody;
    
    void Start()
    {
        _cameraRigidbody = mainCamera.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        var dir = (player.transform.position - _cameraRigidbody.transform.position);
        if (Mathf.Abs(dir.x) < 0.1)
        {
            _cameraRigidbody.velocity = Vector3.zero;
            return;
        }
        if (linearMovement)
            _cameraRigidbody.velocity = Mathf.Sign(dir.x) * cameraMovementSpeed * Vector3.right;
        else 
            _cameraRigidbody.velocity = dir * cameraMovementSpeed;
 
    }
}

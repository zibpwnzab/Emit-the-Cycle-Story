using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2point5Manager : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] Transform mainCamera;
    [SerializeField] float cameraMovementSpeed;
    [SerializeField] bool linearMovement;
    [SerializeField] Vector2 screenShakeInterval;
    [SerializeField] AudioSource audioSource;
    [Header("Fire Wall Settings")]
    [SerializeField] DeadlyObject wall;
    [SerializeField] AnimationCurve curve;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;

    [SerializeField] PlayerController player;

    Rigidbody _cameraRigidbody;
    
    void Start()
    {
        _cameraRigidbody = mainCamera.GetComponent<Rigidbody>();
        StartCoroutine(Shake());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveCamera();
        MoveWall();
    }

    void MoveCamera()
    {
        var dir = (player.transform.position.x - _cameraRigidbody.transform.position.x);
        if (Mathf.Abs(dir) < 0.1)
        {
            _cameraRigidbody.velocity = Vector3.zero;
            return;
        }
        if (linearMovement)
            _cameraRigidbody.velocity = Mathf.Sign(dir) * cameraMovementSpeed * Vector3.right;
        else 
            _cameraRigidbody.velocity = dir * cameraMovementSpeed * Vector3.right;
 
    }

    void MoveWall()
    {
        var dir = endPoint.position.x - wall.transform.position.x;
        if (dir < 0.1) return;
        var speed = curve.Evaluate(dir / (endPoint.position.x - startPoint.position.x));
        wall.transform.Translate(speed * Vector3.right * Time.deltaTime);
    }

    IEnumerator Shake()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(screenShakeInterval.x, screenShakeInterval.y));
            CameraScreenShake.Instance.Shake();
            audioSource.Play();
        }
    }
}

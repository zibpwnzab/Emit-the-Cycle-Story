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
        if (mainCamera != null) _cameraRigidbody = mainCamera.GetComponent<Rigidbody>();
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
        if (player == null || _cameraRigidbody == null) return;
        var dir = (player.transform.position.x - _cameraRigidbody.transform.position.x);
        if (Mathf.Abs(dir) < 0.1)
        {
            _cameraRigidbody.linearVelocity = Vector3.zero;
            return;
        }
        if (linearMovement)
            _cameraRigidbody.linearVelocity = Mathf.Sign(dir) * cameraMovementSpeed * Vector3.right;
        else 
            _cameraRigidbody.linearVelocity = dir * cameraMovementSpeed * Vector3.right;
 
    }

    void MoveWall()
    {
        if (wall == null || startPoint == null || endPoint == null) return;
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
            if (CameraScreenShake.Instance != null) CameraScreenShake.Instance.Shake();
            if (audioSource != null) audioSource.Play();
        }
    }
}

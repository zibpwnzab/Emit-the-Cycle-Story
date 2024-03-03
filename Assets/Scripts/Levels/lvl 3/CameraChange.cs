using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] List<Transform> cameraPoints;
    [SerializeField] float speed;
    [SerializeField] PlayerController player;
    [SerializeField] List<Collider> colliders;
    [SerializeField] List<KeyValuePair<Transform, Collider>> transformColliders;
    int _currentCamera = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayer();
        AnimateCamera();
    }

    public void ChangePosition()
    {
        ChangePosition((_currentCamera + 1) % cameraPoints.Count);
    }
    
    public void ChangePosition(int cameraNumber)
    {
        _currentCamera = cameraNumber % cameraPoints.Count;
    }

    private void AnimateCamera()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPoints[_currentCamera].position,speed * Time.deltaTime);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, cameraPoints[_currentCamera].rotation,speed * Time.deltaTime);
    }

    private void CheckPlayer()
    {
        for (int i = 0; i < colliders.Count; i++)
        { 
            if (colliders[i].bounds.Contains(player.transform.position))
            {
                _currentCamera = i;
                return;
            }
        }
    }
}

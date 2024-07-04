using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float dumping = 1.5f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 3f, -3f);
    [SerializeField] private List<Transform> cameraPoints;
    [SerializeField] private float speed = 2f;
    [SerializeField] private PlayerController player;
    [SerializeField] private List<Collider> colliders;
    [SerializeField] private bool followPlayer = true;
    private int _currentCamera = -1; // -1 means following the player

    void Start()
    {
        if (!player) player = FindAnyObjectByType<PlayerController>();
        if (followPlayer)
        {
            StartCoroutine(FollowPlayer());
        }
    }

    void Update()
    {
        CheckPlayer();
        if (_currentCamera >= 0)
        {
            AnimateCamera();
        }
    }

    public void ChangePosition()
    {
        ChangePosition((_currentCamera + 1) % cameraPoints.Count);
    }

    public void ChangePosition(int cameraNumber)
    {
        _currentCamera = cameraNumber % cameraPoints.Count;
        SetCameraToTargetPosition();
    }

    private void AnimateCamera()
    {
        Vector3 targetPosition = cameraPoints[_currentCamera].position;
        Quaternion targetRotation = cameraPoints[_currentCamera].rotation;

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, speed * Time.deltaTime);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, targetRotation, speed * Time.deltaTime);
    }

    private void CheckPlayer()
    {
        for (int i = 0; i < colliders.Count; i++)
        {
            if (colliders[i].bounds.Contains(player.transform.position))
            {
                ChangePosition(i);
                return;
            }
        }
        if (followPlayer)
        {
            _currentCamera = -1;
        }
    }

    private IEnumerator FollowPlayer()
    {
        while (true)
        {
            if (_currentCamera < 0)
            {
                Vector3 target = new Vector3(player.transform.position.x, player.transform.position.y + offset.y, player.transform.position.z + offset.z);
                Vector3 currentPosition = Vector3.Lerp(mainCamera.transform.position, target, dumping * Time.deltaTime);
                mainCamera.transform.position = currentPosition;
            }
            yield return null;
        }
    }

    private void SetCameraToTargetPosition()
    {
        if (_currentCamera >= 0)
        {
            mainCamera.transform.position = cameraPoints[_currentCamera].position;
            mainCamera.transform.rotation = cameraPoints[_currentCamera].rotation;
        }
    }
}

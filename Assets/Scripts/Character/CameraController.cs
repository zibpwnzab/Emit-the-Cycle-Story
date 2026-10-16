using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float dumping = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 3f, -3f);
    [SerializeField] private PlayerController player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private List<Transform> cameraPoints;
    [SerializeField] private List<Collider> colliders;

    public bool followZOnly = false;

    private void Start()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerController>();
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        if (player == null || mainCamera == null) return;

        Vector3 target = followZOnly
            ? new Vector3(transform.position.x, transform.position.y, player.transform.position.z - offset.z)
            : new Vector3(player.transform.position.x, player.transform.position.y + offset.y, transform.position.z + offset.z);

        mainCamera.transform.position = Vector3.Lerp(
            mainCamera.transform.position,
            target,
            dumping * Time.deltaTime);
    }
}
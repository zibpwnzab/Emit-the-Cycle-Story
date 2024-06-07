using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float dumping;
    [SerializeField] private Vector3 offset = new Vector3(0f, 3f, -3f);
    [SerializeField] private PlayerController player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] List<Transform> cameraPoints;
    [SerializeField] List<Collider> colliders;

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
    }

    void Update()
    {
        Vector3 target;
        target = new Vector3(player.transform.position.x, player.transform.position.y + offset.y, transform.position.z + offset.z);
        Vector3 currentPosition = Vector3.Lerp(mainCamera.transform.position, target, dumping * Time.deltaTime);
        mainCamera.transform.position = currentPosition;
    }
}

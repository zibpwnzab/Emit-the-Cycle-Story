using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dumping;
    public Vector2 offset = new Vector2(2f, 5f);
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Vector3 target;
        target = new Vector3(player.position.x, player.position.y + offset.y, transform.position.z);
        Vector3 currentPosition = Vector3.Lerp(transform.position, target, dumping * Time.deltaTime);
        transform.position = currentPosition;
    }
}

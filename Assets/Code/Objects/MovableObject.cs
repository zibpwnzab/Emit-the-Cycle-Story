using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour, IInteractable
{
    [SerializeField] List<Transform> grabPoints;
    private bool _connected;

    public bool Interact(GameObject otherObject, Animator animator)
    {

        var controller = otherObject.GetComponent<PlayerController>();
        if (_connected)
        {
            transform.parent = null;
            controller.playerState = PlayerState.Walking;
        }
        else
        {
            controller.playerState = PlayerState.MovingObject;
            var dir = transform.position - otherObject.transform.position;
            dir.y = 0;
            dir = dir.normalized;

            var currentForward = otherObject.transform.forward;
            currentForward.y = 0;
            currentForward = currentForward.normalized;

            float angle = Vector3.SignedAngle(dir, currentForward, Vector3.up);

            otherObject.transform.Rotate(Vector3.down, angle);
            transform.parent = otherObject.transform;
        }
        _connected = !_connected;
        
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

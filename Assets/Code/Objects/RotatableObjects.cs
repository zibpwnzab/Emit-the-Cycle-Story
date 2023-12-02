using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatableObjects : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    [SerializeField] List<Transform> grabPoints;
    private bool _connected;

    public bool Interact(GameObject otherObject, Animator animator)
    {


        var controller = otherObject.GetComponent<PlayerController>();
        if (_connected)
        {
            controller.playerState = PlayerState.Walking;
        }
        else
        {
            controller.playerState = PlayerState.RotatingObject;
            var dir = transform.position - otherObject.transform.position;
            dir.y = 0;
            dir = dir.normalized;

            var currentForward = otherObject.transform.forward;
            currentForward.y = 0;
            currentForward = currentForward.normalized;

            float angle = Vector3.SignedAngle(dir, currentForward, Vector3.up);

            otherObject.transform.Rotate(Vector3.down, angle);
        }
        _connected = !_connected;

        return true;
    }
}

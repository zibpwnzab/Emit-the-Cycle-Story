using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Transform> grabPoints;
    [SerializeField] private Collider ColliderToTurnOff;
    [SerializeField] private float distanceFromObject = 1.0f;

    private bool _connected;

    public bool StopInteraction(GameObject otherObject, Animator animator)
    {
        if (_connected)
        {
            return Interact(otherObject, animator);
        }
        return true;
    }

    public bool Interact(GameObject otherObject, Animator animator)
    {
        if (otherObject == null || !otherObject.TryGetComponent(out PlayerController controller))
        {
            return false;
        }

        if (ColliderToTurnOff != null)
        {
            ColliderToTurnOff.enabled = _connected;
        }

        if (_connected)
        {
            transform.parent = null;
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            controller.playerState = PlayerState.Walking;
        }
        else
        {
            controller.playerState = PlayerState.MovingObject;

            Vector3 dir = transform.position - otherObject.transform.position;
            dir.y = 0f;
            dir.Normalize();

            Vector3 currentForward = otherObject.transform.forward;
            currentForward.y = 0f;
            currentForward.Normalize();

            float angle = Vector3.SignedAngle(dir, currentForward, Vector3.up);
            otherObject.transform.Rotate(Vector3.down, angle);

            otherObject.transform.position -= dir * distanceFromObject;
            transform.parent = otherObject.transform;
        }

        _connected = !_connected;
        return true;
    }
}
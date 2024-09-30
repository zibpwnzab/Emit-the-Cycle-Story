using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovableObject : MonoBehaviour, IInteractable
{
    [SerializeField] List<Transform> grabPoints;
    [SerializeField] Collider ColliderToTurnOff;
    private bool _connected;

    public bool StopInteraction(GameObject gameObject, Animator animator)
    {
        if (_connected)
            return Interact(gameObject, animator);
        else return true;
    }

    public bool Interact(GameObject otherObject, Animator animator)
    {
        if (ColliderToTurnOff) ColliderToTurnOff.enabled = _connected;
        var controller = otherObject.GetComponent<PlayerController>();
        if (_connected)
        {
            transform.parent = null;
            // Убедись, что объект вернулся на "корневой" уровень и не наследует DontDestroyOnLoad
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
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

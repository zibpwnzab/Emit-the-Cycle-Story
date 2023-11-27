using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour, IInteractable
{
    private float _originalSpeed;
    [SerializeField] float slowDown;
    [SerializeField] List<Transform> grabPoints;
    private bool _connected;

    public bool Interact(GameObject otherObject, Animator animator)
    {

        var controller = otherObject.GetComponent<PlayerController>();
        if (_connected)
        {
            transform.parent = null;
            controller.moveSpeed = _originalSpeed;
        }
        else
        {
            var c_p = otherObject.transform.position;
            c_p.y = 0;
            var b_wr_c = transform.position - c_p;
            b_wr_c.y = 0;
            float angle = Vector3.Angle(otherObject.transform.forward, b_wr_c);

            otherObject.transform.Rotate(Vector3.down, angle);
            transform.parent = otherObject.transform;
            _originalSpeed = controller.moveSpeed;
            controller.moveSpeed *= slowDown;
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

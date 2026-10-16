using cherrydev;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    private bool _hasTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (_hasTriggered) return;

        if (other.CompareTag("Player") || other.GetComponent<PlayerController>() != null)
        {
            if (GameController.instance != null)
            {
                _hasTriggered = true;
                GameController.instance.EventAnimTrigger();
            }
        }
    }
}
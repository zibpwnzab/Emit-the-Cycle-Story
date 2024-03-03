using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if ((!other.isTrigger) && other.TryGetComponent(out PlayerController player))
        {
            player.Die();
        }
    }
}

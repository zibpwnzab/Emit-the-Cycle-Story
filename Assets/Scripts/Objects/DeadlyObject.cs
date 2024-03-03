using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyObject : MonoBehaviour
{
    [SerializeField]Transform stunDirection;
    [SerializeField]float stunForce;
    [SerializeField]float stunTime;
    private void OnTriggerEnter(Collider other)
    {
        if ((!other.isTrigger) && other.TryGetComponent(out PlayerController player))
        {
            if (player.playerState == PlayerState.Stunned) return;
            if(stunDirection) player.ForceKick(stunDirection.forward * stunForce, stunTime);
            
            player.Lifes -= 1;
            if (player.Lifes <= 0)
            player.Die();
        }
    }
}

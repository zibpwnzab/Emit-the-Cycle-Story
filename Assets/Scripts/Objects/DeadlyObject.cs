using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyObject : MonoBehaviour
{
    [SerializeField]Transform stunDirection;
    [SerializeField]float stunForce;
    [SerializeField]float stunTime;
    [SerializeField] bool needSignal;
    [SerializeField] ISignal signal;
    [SerializeField] int RemovedLifes = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (needSignal) if (!signal.Signal()) return;
        if ((!other.isTrigger) && other.TryGetComponent(out PlayerController player))
        {
            if (player.playerState == PlayerState.Stunned) return;
            if(stunDirection) player.ForceKick(stunDirection.forward * stunForce, stunTime);
            
            player.Lifes -= RemovedLifes;
            if (player.Lifes <= 0)
            player.Die();
        }
    }
}

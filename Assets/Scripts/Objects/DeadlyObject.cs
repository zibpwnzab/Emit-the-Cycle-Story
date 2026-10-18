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
        if (needSignal && (signal == null || !signal.Evaluate())) return;
        if ((!other.isTrigger) && other.TryGetComponent(out PlayerController player))
        {
            if (player.playerState == PlayerState.Stunned || player.playerState == PlayerState.Dead) return;
            if(stunDirection) player.ForceKick(stunDirection.forward * stunForce, stunTime);

            player.TakeDamage(RemovedLifes);
        }
    }
}

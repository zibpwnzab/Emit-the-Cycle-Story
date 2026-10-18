using UnityEngine;
using UnityEngine.Serialization;

public class Bulb : MonoBehaviour
{
    [FormerlySerializedAs("signalObject")]
    [SerializeField] ISignal signalSource;
    [SerializeField] Light lightSource;

    void Update()
    {
        Activate();
    }

    void Activate()
    {
        if (signalSource == null || lightSource == null) return;

        bool isActive = signalSource.Evaluate();
        if (lightSource.gameObject.activeSelf != isActive)
        {
            lightSource.gameObject.SetActive(isActive);
        }
    }
}

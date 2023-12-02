using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour, ISignal
{
    [SerializeField] public ISignal signalSource;
    [SerializeField] public GameObject source;
    [SerializeField] public bool inverseSignal;
    public bool Signal()
    {
        signalSource = source.GetComponent<ISignal>();
        if (inverseSignal)
            return !source.GetComponent<ISignal>().Signal();
        return source.GetComponent<ISignal>().Signal();
    }
}

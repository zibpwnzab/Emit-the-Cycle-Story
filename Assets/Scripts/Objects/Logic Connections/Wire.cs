using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : ISignal
{
    [SerializeField] public ISignal signalSource;
    [SerializeField] public bool inverseSignal;
    override public bool Signal()
    {
        if (inverseSignal)
            return !signalSource.Signal();
        return signalSource.Signal();
    }
}

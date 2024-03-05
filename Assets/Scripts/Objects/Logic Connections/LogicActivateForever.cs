using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicActivateForever : ISignal
{
    [SerializeField] ISignal signal;
    [SerializeField] bool inverse;
    bool Power;
    public override bool Signal()
    {
        if (inverse)
            return !Power;
        return Power;
    }

    void Update()
    {
        if (Power) return;
        Power = signal.Signal();
    }
}

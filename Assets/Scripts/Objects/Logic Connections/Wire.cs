using UnityEngine;

public class Wire : ISignal
{
    public ISignal signalSource;
    public bool inverseSignal;

    public override bool Signal()
    {
        if (signalSource == null) return false;
        bool state = signalSource.Signal();
        return inverseSignal ? !state : state;
    }
}
using UnityEngine;

public class Wire : ISignal
{
    public ISignal signalSource;
    public bool inverseSignal;

    public override bool Signal()
    {
        if (signalSource == null) return false;
        bool state = signalSource.Evaluate();
        return inverseSignal ? !state : state;
    }

    private void OnValidate()
    {
        if (signalSource == this)
        {
            Debug.LogWarning($"Wire '{name}' uses itself as a signal source.", this);
        }
    }
}

using UnityEngine;

public abstract class ISignal : MonoBehaviour
{
    private int _cachedFrame = -1;
    private bool _cachedValue;
    private bool _isEvaluating;

    public abstract bool Signal();

    /// <summary>Cached per frame; a cycle in the signal graph is reported instead of overflowing the stack.</summary>
    public bool Evaluate()
    {
        int frame = Time.frameCount;
        if (_cachedFrame == frame) return _cachedValue;

        if (_isEvaluating)
        {
            Debug.LogError($"Signal cycle detected at '{name}'. Check the signal sources in the Inspector.", this);
            return false;
        }

        _isEvaluating = true;
        try
        {
            _cachedValue = Signal();
            _cachedFrame = frame;
        }
        finally
        {
            _isEvaluating = false;
        }

        return _cachedValue;
    }
}

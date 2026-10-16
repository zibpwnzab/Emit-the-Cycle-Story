using UnityEngine;

public class LogicActivateForever : ISignal
{
    [SerializeField] private ISignal signal;
    [SerializeField] private bool inverse;

    private bool _isActivated;

    public override bool Signal()
    {
        return inverse ? !_isActivated : _isActivated;
    }

    private void Update()
    {
        if (_isActivated || signal == null) return;

        if (signal.Signal())
        {
            _isActivated = true;
        }
    }
}
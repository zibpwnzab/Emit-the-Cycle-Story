using UnityEngine;

public class LogicTimer : ISignal
{
    [SerializeField] private ISignal incomingSignal;
    [SerializeField] private float delay = 1f;
    [SerializeField] private float signalTime = 0.1f;

    private float _delayLeft;
    private bool _isPowered => _delayLeft <= 0f;

    public override bool Signal()
    {
        return _isPowered;
    }

    private void Start()
    {
        _delayLeft = delay;
    }

    private void Update()
    {
        if (_isPowered || incomingSignal == null) return;

        if (incomingSignal.Signal())
        {
            _delayLeft -= Time.deltaTime;
        }
    }
}
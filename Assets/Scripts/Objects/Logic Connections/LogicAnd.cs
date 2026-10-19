using System.Collections.Generic;
using UnityEngine;

public class LogicAnd : ISignal
{
    private enum GateType
    {
        AND,
        OR,
        NOR,
        NAND
    }

    [SerializeField] private GateType gate;
    [SerializeField] private List<GameObject> sources = new List<GameObject>();

    private readonly List<ISignal> _cachedSignals = new List<ISignal>();

    private void Awake()
    {
        CacheSignalSources();
    }

    private void OnEnable()
    {
        CacheSignalSources();
    }

    private void OnValidate()
    {
        if (sources != null && sources.Contains(gameObject))
        {
            Debug.LogWarning($"Logic gate '{name}' lists its own GameObject as a source.", this);
        }
    }

    private void CacheSignalSources()
    {
        _cachedSignals.Clear();
        if (sources == null) return;

        foreach (GameObject sourceObj in sources)
        {
            if (sourceObj != null && sourceObj.TryGetComponent(out ISignal signal))
            {
                _cachedSignals.Add(signal);
            }
        }
    }

    public override bool Signal()
    {
        return gate switch
        {
            GateType.AND => EvaluateAnd(),
            GateType.OR => EvaluateOr(),
            GateType.NOR => !EvaluateOr(),
            GateType.NAND => !EvaluateAnd(),
            _ => false
        };
    }

    private bool EvaluateAnd()
    {
        if (_cachedSignals.Count == 0) return false;

        foreach (ISignal signal in _cachedSignals)
        {
            if (signal == null || !signal.Evaluate())
            {
                return false;
            }
        }
        return true;
    }

    private bool EvaluateOr()
    {
        foreach (ISignal signal in _cachedSignals)
        {
            if (signal != null && signal.Evaluate())
            {
                return true;
            }
        }
        return false;
    }
}
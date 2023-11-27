using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicAnd : MonoBehaviour, ISignal
{
    [SerializeField] GateType gate;
    [SerializeField] List<GameObject> sources;
    public bool Signal()
    {
        switch (gate){
            case GateType.AND:
                return And();
            case GateType.OR:
                return Or();
            case GateType.NOR:
                return !Or();
            default:
                return !And();
        }
    }
    private bool And()
    {
        foreach (var source in sources)
            if (!source.GetComponent<ISignal>().Signal())
                return false;
        return true;
    }

    private bool Or()
    {
        foreach (var source in sources)
            if (source.GetComponent<ISignal>().Signal())
                return true;
        return false;
    }
    private enum GateType
    {
        AND,
        OR,
        NOR,
        NAND,
    }
}


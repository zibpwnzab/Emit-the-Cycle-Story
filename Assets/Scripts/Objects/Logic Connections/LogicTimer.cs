using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicTimer : ISignal
{
    [SerializeField] ISignal incomingSignal;
    [SerializeField] float delay;
    [SerializeField] float signalTime = 0.1f;

    float _signalLeft;
    float _delayLeft;
    bool powered => (_delayLeft <= 0);
    public override bool Signal()
    {
        return powered;
    }

    void Start()
    {
        _delayLeft = delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (powered) return;
        if (incomingSignal.Signal())
        {
            _delayLeft -= Time.deltaTime;
        }
    }
}

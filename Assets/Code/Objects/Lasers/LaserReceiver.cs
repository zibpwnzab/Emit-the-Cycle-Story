using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReceiver : MonoBehaviour, ISignal
{

    public bool powered;
    Queue<bool> prevStates = new Queue<bool>();
    private int queueCapacity = 10;
    public bool Signal()
    {
        foreach (bool b in prevStates)
            if (b) return true;
        return false;
    }

    public void Power(bool b)
    {
        if (prevStates.Count >= queueCapacity)
            prevStates.Dequeue();
        prevStates.Enqueue(b);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Power(false);
    }
}

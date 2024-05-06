using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReceiver : ISignal
{

    public bool powered;
    Queue<bool> prevStates = new Queue<bool>();
    private int queueCapacity = 10;
    [SerializeField] MeshRenderer meshToLight;
    [SerializeField] Animator animator;
    [SerializeField] Material onMaterial;
    [SerializeField] Material offMaterial;
    override public bool Signal()
    {
        foreach (bool b in prevStates) 
        if (b) return true;
        return false;
    }

    public void Power(bool b)
    {
        if (animator) {
            if (Signal())animator.SetFloat("speed", 1);
        else animator.SetFloat("speed", 0);
        }
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
        if (meshToLight)
        {
            if(Signal())
            {
                meshToLight.material = onMaterial;
                powered = true;
            }
            else
            {
                meshToLight.material = offMaterial;
                powered = false;
            }
        }
    }
}

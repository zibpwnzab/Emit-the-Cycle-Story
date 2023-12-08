using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bulb : MonoBehaviour
{
    [SerializeField] GameObject signalObject;
    [SerializeField] Light lightSource;
    ISignal signal;
    void Start()
    {
        signal = signalObject.GetComponent<ISignal>();
    }

    // Update is called once per frame
    void Update()
    {
        Activate();
    }

    void Activate()
    {
        lightSource.gameObject.SetActive(signal.Signal());
    }
}

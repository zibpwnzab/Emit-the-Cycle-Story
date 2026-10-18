using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class Bulb : MonoBehaviour
{
    [FormerlySerializedAs("signalObject")]
    [SerializeField] ISignal signalSource;
    [SerializeField] Light lightSource;


    // Update is called once per frame
    void Update()
    {
        Activate();
    }

    void Activate()
    {
        lightSource.gameObject.SetActive(signalSource.Signal());
    }
}

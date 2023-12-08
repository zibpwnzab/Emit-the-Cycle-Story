using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject signalObject;
    [SerializeField] Light lightSource;
    [SerializeField] int SceneNumber;
    ISignal signal;
    [SerializeField] bool needsSignal;
    void Start()
    {
        if (needsSignal)
        signal = signalObject.GetComponent<ISignal>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool Interact(GameObject gameObject, Animator animator)
    {
        if (needsSignal)
        {
            if (!signal.Signal()) return false;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNumber);
        return true;
    }
}

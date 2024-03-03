using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour, IInteractable
{
    [SerializeField] ISignal signalSource;
    [SerializeField] int SceneNumber;
    [SerializeField] bool needsSignal;


    // Update is called once per frame
    public bool StopInteraction(GameObject gameObject, Animator animator)
    {
        return Interact(gameObject, animator);
    }
    public bool Interact(GameObject gameObject, Animator animator)
    {
        if (needsSignal)
        {
            if (!signalSource.Signal()) return false;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNumber);
        return true;
    }
}

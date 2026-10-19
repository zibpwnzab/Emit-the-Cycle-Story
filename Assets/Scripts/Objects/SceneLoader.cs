using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, IInteractable
{
    [SerializeField] private List<ISignal> signalSources;
    [SerializeField] private string sceneName;
    [SerializeField] private bool needsSignal = true;
    [SerializeField] private bool useTrigger = true;
    [SerializeField] private bool useTimer = false;
    [SerializeField] private float delayBeforeLoad = 5.0f;

    private bool _sceneLoadingTriggered;

    private void Start()
    {
        if (useTimer)
        {
            StartCoroutine(LoadSceneAfterDelay());
        }
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        LoadNextScene(null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (useTrigger && other.CompareTag("Player"))
        {
            LoadNextScene(other.gameObject);
        }
    }

    public bool StopInteraction(GameObject gameObject, Animator animator)
    {
        return true;
    }

    public bool Interact(GameObject gameObject, Animator animator)
    {
        if (needsSignal && !AreAllSignalsActive())
        {
            return false;
        }

        LoadNextScene(gameObject);
        return true;
    }

    private bool AreAllSignalsActive()
    {
        if (signalSources == null) return true;

        foreach (var signal in signalSources)
        {
            if (signal == null || !signal.Evaluate())
            {
                return false;
            }
        }
        return true;
    }

    private void LoadNextScene(GameObject player)
    {
        if (_sceneLoadingTriggered || string.IsNullOrEmpty(sceneName)) return;

        _sceneLoadingTriggered = true;

        if (player != null)
        {
            Destroy(player);
        }

        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneFromUIButton()
    {
        LoadSceneFromUIButton(null);
    }

    public void LoadSceneFromUIButton(GameObject player)
    {
        if (_sceneLoadingTriggered) return;
        if (needsSignal && !AreAllSignalsActive()) return;

        LoadNextScene(player);
    }
}
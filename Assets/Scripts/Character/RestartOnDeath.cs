using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float delayBeforeRestart = 2.0f;

    private bool _isRestarting;

    private void Update()
    {
        if (player == null && !_isRestarting)
        {
            StartCoroutine(RestartSceneRoutine());
        }
    }

    private IEnumerator RestartSceneRoutine()
    {
        _isRestarting = true;
        yield return new WaitForSeconds(delayBeforeRestart);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
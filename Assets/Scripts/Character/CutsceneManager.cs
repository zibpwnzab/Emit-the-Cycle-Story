using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public UnityEvent onCutsceneFinished;

    private void Start()
    {
        if (playableDirector == null)
        {
            playableDirector = GetComponent<PlayableDirector>();
        }

        if (playableDirector != null)
        {
            playableDirector.stopped += OnPlayableDirectorStopped;
            playableDirector.Play();
        }
    }

    private void OnDestroy()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped -= OnPlayableDirectorStopped;
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            StartGameplay();
        }
    }

    private void StartGameplay()
    {
        onCutsceneFinished?.Invoke();
    }
}
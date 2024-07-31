using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector playableDirector;

    void Start()
    {
        playableDirector.stopped += OnPlayableDirectorStopped;
        playableDirector.Play();
    }

    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            StartGameplay();
        }
    }

    void StartGameplay()
    {
        // Логика для начала геймпле
        Debug.Log("Gameplay started");
    }
}

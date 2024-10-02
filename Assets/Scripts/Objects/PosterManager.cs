using UnityEngine;

public class PosterManager : MonoBehaviour
{
    public static PosterManager instance;
    public int collectedPosters = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectPoster()
    {
        collectedPosters++;
        Debug.Log("Poster collected! Total posters: " + collectedPosters);
    }
}

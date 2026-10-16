using UnityEngine;

public class PlayerLifes : MonoBehaviour
{
    [SerializeField] private GameObject heartImage1;
    [SerializeField] private GameObject heartImage2;
    [SerializeField] private GameObject heartImage3;
    [SerializeField] private GameObject gameoverImage;

    private PlayerController _player;
    private int _lastDisplayedLives = -1;

    private void Start()
    {
        _player = FindFirstObjectByType<PlayerController>();
        RefreshHearts();
    }

    private void Update()
    {
        if (_player != null && _player.Lifes != _lastDisplayedLives)
        {
            RefreshHearts();
        }
    }

    private void RefreshHearts()
    {
        if (_player == null) return;
        _lastDisplayedLives = _player.Lifes;

        if (heartImage3 != null) heartImage3.SetActive(_lastDisplayedLives >= 3);
        if (heartImage2 != null) heartImage2.SetActive(_lastDisplayedLives >= 2);
        if (heartImage1 != null) heartImage1.SetActive(_lastDisplayedLives >= 1);
        if (gameoverImage != null) gameoverImage.SetActive(_lastDisplayedLives <= 0);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraScreenShake : MonoBehaviour
{
    [SerializeField] Vector2 intervalRange = Vector2.up;
    [SerializeField] Vector2 shakeRange = Vector2.up;
    [SerializeField] float shakePower = 0;
    [SerializeField] bool controlledByOtherScript;
    public static CameraScreenShake Instance;

    private Tween _shakeTween;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if (!controlledByOtherScript) StartCoroutine(SelfShake());
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void Shake()
    {
        if (!controlledByOtherScript) return;
        StartShake();
    }

    IEnumerator SelfShake()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(intervalRange.x, intervalRange.y));
            StartShake();
        }
    }

    private void StartShake()
    {
        // Finish the previous shake first, otherwise overlapping shakes make the camera drift
        if (_shakeTween != null && _shakeTween.IsActive()) _shakeTween.Complete();
        _shakeTween = transform.DOShakePosition(Random.Range(shakeRange.x, shakeRange.y), shakePower)
            .SetLink(gameObject);
    }
}

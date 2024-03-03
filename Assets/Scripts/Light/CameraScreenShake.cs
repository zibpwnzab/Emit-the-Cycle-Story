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

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if (!controlledByOtherScript) StartCoroutine(SelfShake());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shake()
    {
        if (!controlledByOtherScript) return;
        transform.DOShakePosition(Random.Range(shakeRange.x, shakeRange.y), shakePower);
    }

    IEnumerator SelfShake()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(intervalRange.x, intervalRange.y));
            transform.DOShakePosition(Random.Range(shakeRange.x, shakeRange.y), shakePower);
        }
    }
}

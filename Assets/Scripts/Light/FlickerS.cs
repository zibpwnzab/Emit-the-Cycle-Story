using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerS : MonoBehaviour
{

    Light testLight;   
    public float minWaitTime;
    public float maxWaitTime;
    [SerializeField] bool turnOff;
    [SerializeField] Vector2 intensityRanges = Vector2.one;
    [SerializeField] Vector3 positionOffset;

    Vector3 _startPos;

    void Start()
    {
        _startPos = transform.localPosition;
        TryGetComponent<Light>(out testLight);
        StartCoroutine(Flashing());
    }

    IEnumerator Flashing()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            transform.localPosition = _startPos + Vector3.Scale(Random.insideUnitSphere, positionOffset);
            if (!testLight) yield return null;
            testLight.enabled = !testLight.enabled || !turnOff;
            testLight.intensity = Random.Range(intensityRanges.x, intensityRanges.y);
            
        }
    }
}

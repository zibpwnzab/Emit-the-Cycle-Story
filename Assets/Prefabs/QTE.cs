using UnityEngine;
using UnityEngine.UI;

public class QTE : MonoBehaviour
{
    private const float StartDelay = 2f;
    // ~0.02 per frame at 60 FPS, frame-rate independent
    private const float DecayPerSecond = 1.2f;

    public Slider slider;

    private float _decayStartTime;

    void Start()
    {
        if (slider != null) slider.value = 0f;
        _decayStartTime = Time.time + StartDelay;
    }

    private void Update()
    {
        if (slider == null || Time.time < _decayStartTime) return;

        if (slider.value != 10 && slider.value != 0)
        {
            slider.value -= DecayPerSecond * Time.deltaTime;
        }
    }

    public void ValueChange()
    {
        if (slider != null) slider.value += 1;
    }
}

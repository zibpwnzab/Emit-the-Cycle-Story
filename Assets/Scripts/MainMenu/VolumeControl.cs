using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public string volumeParametr = "MasterVolume";
    public AudioMixer mixer;
    public Slider slider;

    private const float _multiplier = 20f;

    private void Awake()
    {
        if (slider != null) slider.onValueChanged.AddListener(HandleSliderValueChanged);
    }

    private void HandleSliderValueChanged(float value) 
    {
        if (mixer == null) return;
        // Log10(0) = -Infinity, clamp to -80 dB
        var volumeValue = Mathf.Log10(Mathf.Max(value, 0.0001f)) * _multiplier;
        mixer.SetFloat(volumeParametr, volumeValue);
    }
    void Start()
    {
        
    }

   
}

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
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
    }

    private void HandleSliderValueChanged(float value) 
    {
        var volumeValue = Mathf.Log10(value) * _multiplier;
        mixer.SetFloat(volumeParametr, volumeValue);
    }
    void Start()
    {
        
    }

   
}

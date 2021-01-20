using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [SerializeField] SoundType soundType;
    [SerializeField] Slider slider;
    [SerializeField] [SerializeInterface(typeof(SoundManager))] SoundManager soundManager;

    private void OnEnable()
    {
        if (slider)
            slider.value = soundManager.GetInstance().GetVolume(soundType);
    }

    public void SetVolume(float value)
    {
        soundManager.GetInstance().SetVolume(value, soundType);
    }
}
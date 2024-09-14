using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControlScript : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 1);
            SetSliderValue();
        }
        else
        {
            SetSliderValue();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = slider.value;
        PlayerPrefs.SetFloat("volume", slider.value);
    }

    private void SetSliderValue()
    {
        slider.value = PlayerPrefs.GetFloat("volume");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        LoadVolumeValues();
    }

    public void SaveVolume()
    {
        float value = volumeSlider.value;
        PlayerPrefs.SetFloat("volume",value);
        LoadVolumeValues();
    }

    public void LoadVolumeValues()
    {
        float value = PlayerPrefs.GetFloat("volume", 0.5f);
        volumeSlider.value = value;
        AudioListener.volume = value;
    }
}

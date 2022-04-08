using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    public SettingsScriptableObject settings;
    public Slider volumeSlider;
    public Dropdown resolutionPicker;
    public Toggle fullScreenToggle;
    public AudioMixer audioMixer;

    private void Start()
    {
        settings.Initialize();
        List<string> options = new List<string>();
        foreach (ResolutionInfo info in settings.resolutions) {
            options.Add(string.Format("{0} x {1}", info.widthHeight.x, info.widthHeight.y));
        }
        resolutionPicker.AddOptions(options);
        resolutionPicker.value = settings.currIndex;
        resolutionPicker.RefreshShownValue();

        fullScreenToggle.isOn = settings.isFullScreen;

        volumeSlider.value = settings.volumeSliderValue;
        SetVolume(settings.volumeSliderValue);
    }

    public void ToggleFullScreen(bool isFullScreen) {
        settings.SetFullScreen(isFullScreen);
    }
    
    public void PickResolution(int index) {
        settings.SetResolutionIndex(index);
        resolutionPicker.value = settings.currIndex;
        resolutionPicker.RefreshShownValue();
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume)*20);
        settings.volumeSliderValue = volume;
    }
}

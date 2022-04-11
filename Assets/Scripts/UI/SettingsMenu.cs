using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{

    public SettingsScriptableObject settings;
    public TMP_Dropdown resolutionPicker;
    public Toggle fullScreenToggle;
    public AudioMixer audioMixer;

    private void Start()
    {

        if (resolutionPicker != null) {
            List<string> options = new List<string>();
            foreach (ResolutionInfo info in settings.resolutions) {
                options.Add(string.Format("{0} x {1}", info.widthHeight.x, info.widthHeight.y));
            }
            resolutionPicker.ClearOptions();
            resolutionPicker.AddOptions(options);
            resolutionPicker.SetValueWithoutNotify(settings.findSetCurrIndex());
            resolutionPicker.RefreshShownValue();
        }

        if (fullScreenToggle != null) {
            fullScreenToggle.isOn = Screen.fullScreen;
        }

        // SetVolume(0);
    }

    public void ToggleFullScreen(bool isFullScreen) {
        settings.SetFullScreen(isFullScreen);
    }
    
    public void PickResolution(int index) {
        settings.SetResolutionIndex(index);
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume)*20);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{

    private List<Resolution> resolutions;
    public TMP_Dropdown resolutionPicker;
    public Toggle fullScreenToggle;
    public AudioMixer audioMixer;
    public static readonly string FullScreenPref = "FullScreenPref";
    public static readonly string ResolutionWidthPref = "ResolutionWidthPref";
    public static readonly string ResolutionHeightPref = "ResolutionHeightPref";

    private void Start() {
        resolutions = new List<Resolution>();
        int pickedIndex = 0;

        if (resolutionPicker != null) {
            List<string> options = new List<string>();
            // foreach (Resolution info in resolutions) {
            for (int i = Screen.resolutions.Length - 1; i >= 0; i--) {
                var info = Screen.resolutions[i];
                resolutions.Add(info);
                options.Add(string.Format("{0} x {1}", info.width, info.height));
                if (Screen.currentResolution.height == info.height && 
                    Screen.currentResolution.width == info.width) pickedIndex = i;
            }
            resolutionPicker.ClearOptions();
            resolutionPicker.AddOptions(options);
            resolutionPicker.SetValueWithoutNotify(pickedIndex);
            Screen.SetResolution(resolutions[pickedIndex].width, resolutions[pickedIndex].height, 
                !fullScreenToggle || fullScreenToggle.isOn);
            resolutionPicker.RefreshShownValue();
        }

        // SetVolume(0);
    }

    public void ToggleFullScreen(bool isFullScreen) {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt(FullScreenPref, isFullScreen ? 1 : 0);
        if (fullScreenToggle != null) fullScreenToggle.SetIsOnWithoutNotify(isFullScreen);
    }
    
    public void PickResolution(int index) {
        Debug.Log($"resolution picked {resolutions[index].width}, {resolutions[index].height}");
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);

        PlayerPrefs.SetInt(ResolutionWidthPref, resolutions[index].width);
        PlayerPrefs.SetInt(ResolutionHeightPref, resolutions[index].height);
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume)*20);
        // immediately save pref volume
        PlayerPrefs.SetFloat(AudioManager.VolumePref, volume);
    }
}

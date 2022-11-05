using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class SettingsMenu : MonoBehaviour {

    private PostProcessOutline outlineSettings;
    private List<Resolution> resolutions;
    public TMP_Dropdown resolutionPicker;
    public Toggle fullScreenToggle;
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public Slider outlineSlider;
    public static readonly string FullScreenPref = "FullScreenPref";
    public static readonly string ResolutionWidthPref = "ResolutionWidthPref";
    public static readonly string ResolutionHeightPref = "ResolutionHeightPref";
    public static readonly string OutlineDepthThreshold = "OutlineDepthThreshold";

    private void Awake() {
        PostProcessVolume postProcessVolume = Camera.main.GetComponent<PostProcessVolume>();
        if (postProcessVolume) {
            postProcessVolume.profile.TryGetSettings<PostProcessOutline>(out outlineSettings);
            LoadOutlineValue();
        }
        LoadVolume();

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
            resolutionPicker.RefreshShownValue();
        }

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

    public void SetVolume (float volume){
        audioMixer.SetFloat("volume", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat(AudioManager.VolumePref, volume);
    }

    public void LoadVolume() {
        if (PlayerPrefs.HasKey(AudioManager.VolumePref) && volumeSlider) {
            volumeSlider.value = PlayerPrefs.GetFloat(AudioManager.VolumePref);
        }
    }

    public void SetOutlineValue(float invertValue) {
        float value = 1 - invertValue;
        if (outlineSettings) outlineSettings.depthThreshold.value = value;
        PlayerPrefs.SetFloat(OutlineDepthThreshold, value);

        Debug.Log($"value set to {value}");
    }

    public void LoadOutlineValue() {
        if (PlayerPrefs.HasKey(OutlineDepthThreshold) && outlineSlider) {
            float value = PlayerPrefs.GetFloat(OutlineDepthThreshold);
            outlineSettings.depthThreshold.value = value;
            outlineSlider.value = 1 - value;
        }
    }
}

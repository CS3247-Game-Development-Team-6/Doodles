using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class SettingsMenu : MonoBehaviour {

    private PostProcessOutline outlineSettings;
    private PostProcessBundle outlineBundle;
    private PostProcessVolume postProcessVolume;
    private List<Resolution> resolutions;
    public TMP_Dropdown resolutionPicker;
    public Toggle fullScreenToggle;
    public AudioMixer audioMixer;
    public static readonly string FullScreenPref = "FullScreenPref";
    public static readonly string ResolutionWidthPref = "ResolutionWidthPref";
    public static readonly string ResolutionHeightPref = "ResolutionHeightPref";
    public static readonly string OutlineDepthThreshold = "OutlineDepthThreshold";

    private void Awake() {
        postProcessVolume = Camera.main.GetComponent<PostProcessVolume>();
        // outlineBundle = postProcessVolume.GetBundle<PostProcessOutline>();
        // Debug.Log("OKAYU" + outlineBundle);
        // outlineSettings = (PostProcessOutline)outlineBundle.settings;
        postProcessVolume.profile.TryGetSettings<PostProcessOutline>(out outlineSettings);

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

    public void SetVolume (float volume){
        audioMixer.SetFloat("volume", Mathf.Log10(volume)*20);
        // immediately save pref volume
        PlayerPrefs.SetFloat(AudioManager.VolumePref, volume);
    }

    public void SetOutlineValue(float value) {
        outlineSettings.depthThreshold.value = value;
        // postProcessLayer.Render()
        PlayerPrefs.SetFloat(OutlineDepthThreshold, value);

        Debug.Log($"value set to {value}");
    }

    public void LoadOutlineValue() {
        if (PlayerPrefs.HasKey(OutlineDepthThreshold)) {
            float value = PlayerPrefs.GetFloat(OutlineDepthThreshold);
             outlineSettings.depthThreshold.value = value;
        }
    }
}

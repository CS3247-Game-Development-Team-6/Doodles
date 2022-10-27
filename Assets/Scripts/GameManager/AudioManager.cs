using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {
    private static readonly string FirstPlay = "FirstPlay";
    public static readonly string VolumePref = "VolumePref";
    private int firstPlayInt;
    public Slider volumeSlider;
    private float volumeFloat;
    public Sound[] sounds;
    
    void Awake() {
        if (firstPlayInt == 0) {
            foreach (Sound s in sounds) {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;

                s.source.outputAudioMixerGroup = s.group;
            }
        } else {
            ContinueSettings();
        }
    }

    private void Start() {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayInt == 0) {
            volumeFloat = 0.6f;
            volumeSlider.value = volumeFloat;
            PlayerPrefs.SetFloat(VolumePref, volumeFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);
        } else {
            volumeFloat = PlayerPrefs.GetFloat(VolumePref);
            volumeSlider.value = volumeFloat;
        }

        if (SceneManager.GetActiveScene().name.StartsWith("Menu-beta"))
            Play("Main Menu BGM");
        else if (SceneManager.GetActiveScene().name.StartsWith("Loadout"))
            Play("Loadout BGM");

    }

    public void SaveSoundSettings() {
        PlayerPrefs.SetFloat(VolumePref, volumeSlider.value);
    }

    // Save Player Pref values when not in focus or player exits
    private void OnApplicationFocus(bool inFocus) {
        if (!inFocus) {
            SaveSoundSettings();
        }
    }

    public void UpdateSound() {
        foreach (Sound s in sounds) {
            s.source.volume = volumeSlider.value;

        }
    }

    public void Play (string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        // Sound not found
        if (s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void Stop (string sound) {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void ContinueSettings() {
        volumeFloat = PlayerPrefs.GetFloat(VolumePref);

        foreach (Sound s in sounds) {
            s.source.volume = volumeFloat;
        }
    }
}

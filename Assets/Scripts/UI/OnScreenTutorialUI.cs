using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Note {
    public string text;
    public Vector3 rectPosition;
}

public class OnScreenTutorialUI : MonoBehaviour {

    public Note[] notes;
    public int currentIndex = 0;
    public TextMeshProUGUI textBox;

    private int defaultTextLength = -1;
    private float defaultFontSize = -1;
    // private static readonly string OnScreenTutorialPref = "OnScreenTutorialPref";

    // for wavespawner, assuming not to be set false each start()
    public static bool hasSeenTutorial = false;

    private void Start() {
        /*
        if (PlayerPrefs.HasKey(OnScreenTutorialPref) && PlayerPrefs.GetInt(OnScreenTutorialPref) == 0) {
            gameObject.SetActive(false);
            return;
        }
        */
        if (defaultTextLength < 0) {
            defaultTextLength = textBox.text.Length;
            defaultFontSize = textBox.fontSize;
        }
        SetIndex(0);
    }

    public void ReEnable() {
        gameObject.SetActive(true);
        if (defaultTextLength < 0) {
            defaultTextLength = textBox.text.Length;
            defaultFontSize = textBox.fontSize;
        }
        SetIndex(0);
    }

    private float getFontMultiplier(string text) {
        return Mathf.Min(1f, 1.1f * Mathf.Sqrt((float)defaultTextLength / text.Length));
    }

    private void SetText(string text) {
        textBox.text = text;
        textBox.fontSize = defaultFontSize * getFontMultiplier(text);
    }

    public void SetNextIndex() {
        if (currentIndex >= notes.Length - 1) {
            Hide();
        } else {
            currentIndex += 1;
            SetText(notes[currentIndex].text);
            GetComponent<RectTransform>().anchoredPosition
                = notes[currentIndex].rectPosition;
        }
    }

    public void SetIndex(int index) {
        if (index >= notes.Length) {
            Hide();
        } else {
            currentIndex = index;
            SetText(notes[currentIndex].text);
            GetComponent<RectTransform>().anchoredPosition
                = notes[currentIndex].rectPosition;
        }
    }

    public void Hide() {
        gameObject.SetActive(false);
        // Saves the fact that player has seen this tutorial already
        // PlayerPrefs.SetInt(OnScreenTutorialPref, 0);
        hasSeenTutorial = true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Note {
    [TextArea(4,6)]
    public string text;
    public Vector3 rectPosition;
}

public class OnScreenTutorialUI : MonoBehaviour {

    public Note[] notes;
    public int currentIndex = 0;
    public TextMeshProUGUI textBox;

    private int defaultTextLength = -1;
    private float defaultFontSize = -1;
    public static readonly string OnScreenTutorialPref = "OnScreenTutorialPref";

    private void Start() {
        // Do not uncomment: only for debugging.
        /*
        if (PlayerPrefs.HasKey(OnScreenTutorialPref) && PlayerPrefs.GetInt(OnScreenTutorialPref) == 1) {
            gameObject.SetActive(false);
            return;
        }
        */

        PlayerPrefs.DeleteKey(OnScreenTutorialPref);

        if (defaultTextLength < 0) {
            defaultTextLength = textBox.text.Length;
            defaultFontSize = textBox.fontSize;
        }
        SetIndex(0);
    }

    public void SetNotes(ChunkInfoScriptableObject chunkInfo) {
        this.notes = chunkInfo.notes;
        SetIndex(0);
    }

    public void ReEnable() {
        PlayerPrefs.DeleteKey(OnScreenTutorialPref);

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

    public void SetPrevIndex() {
        if (currentIndex == 0) {
            return;
        } else {
            currentIndex -= 1;
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
        PlayerPrefs.SetInt(OnScreenTutorialPref, 1);
    }
}
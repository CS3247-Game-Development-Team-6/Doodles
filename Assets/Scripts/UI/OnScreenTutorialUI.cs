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

    private int defaultTextLength;
    private float defaultFontSize;
    private static readonly string OnScreenTutorialPref = "OnScreenTutorialPref";

    private void Start() {
        if (PlayerPrefs.HasKey(OnScreenTutorialPref) && PlayerPrefs.GetInt(OnScreenTutorialPref) == 0)  {
            gameObject.SetActive(false);
            return;
        }
        defaultTextLength = textBox.text.Length;
        defaultFontSize = textBox.fontSize;
        SetIndex(0);
    }

    public void ReEnable() {
        gameObject.SetActive(true);
        defaultTextLength = textBox.text.Length;
        defaultFontSize = textBox.fontSize;
        SetIndex(0);
    }

    private void SetText(string text) {
        textBox.text = text;
        textBox.fontSize = defaultFontSize * Mathf.Min(1f, Mathf.Sqrt((float)defaultTextLength / text.Length));
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
        PlayerPrefs.SetInt(OnScreenTutorialPref, 0);
    }
}

using System;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Note {
    [TextArea(4,6)]
    public string text;
    public Vector3 rectPosition;
}

[RequireComponent(typeof(CanvasGroup))]
public class OnScreenTutorialUI : MonoBehaviour {

    private Note[] notes;
    public int currentIndex = 0;
    public TextMeshProUGUI textBox;

    private int defaultTextLength = -1;
    private float defaultFontSize = -1;
    public bool IsClosed { get; private set; } = false;

    private void Start() {
        // Do not uncomment: only for debugging.
        /*
        if (PlayerPrefs.HasKey(OnScreenTutorialPref) && PlayerPrefs.GetInt(OnScreenTutorialPref) == 1) {
            gameObject.SetActive(false);
            return;
        }

        PlayerPrefs.DeleteKey(OnScreenTutorialPref);
        */
        notes = new Note[0];

        Map map = FindObjectOfType<Map>();
        if (map != null && map.currentChunk != null) {
            notes = map.currentChunk.levelInfo.notes;
        }

        if (defaultTextLength < 0) {
            defaultTextLength = textBox.text.Length;
            defaultFontSize = textBox.fontSize;
        }

        if (notes.Length > 0) SetIndex(0);
    }

    public void SetNotes(ChunkInfoScriptableObject chunkInfo) {
        this.notes = chunkInfo.notes;
        SetIndex(0);
    }

    // Triggered OnWaveEnd (from Chunk)
    public void SetNotesForNextChunk(object sender, EventArgs e) {
        if (!(sender is ChunkSpawner)) return;
        Chunk currChunk = ((ChunkSpawner)sender).GetComponent<Chunk>();
        if (currChunk.nextChunk == null) return;
        SetNotes(currChunk.nextChunk.levelInfo);
        Debug.Log($"Setting the notes for next chunk {currChunk.nextChunk}");
    }

    private void SetText(string text) {
        textBox.text = text;
    }

    public void SetNextIndex() {
        currentIndex = Mathf.Min(currentIndex+1, notes.Length);
        if (currentIndex < notes.Length) {
            SetText(notes[currentIndex].text);
            GetComponent<RectTransform>().anchoredPosition
                = notes[currentIndex].rectPosition;
        } else {
            IsClosed = true;
        }
    }

    public void SetPrevIndex() {
        currentIndex = Mathf.Max(currentIndex-1, -1);
        if (currentIndex > 0) {
            SetText(notes[currentIndex].text);
            GetComponent<RectTransform>().anchoredPosition
                = notes[currentIndex].rectPosition;
        } else {
            IsClosed = true;
        }
    }

    public void SetIndex(int index) {
        currentIndex = index;
        if (currentIndex < 0 || currentIndex >= notes.Length) {
            IsClosed = true;
        }
        SetText(notes[currentIndex].text);
        GetComponent<RectTransform>().anchoredPosition
            = notes[currentIndex].rectPosition;
    }

    public void Hide() {
        IsClosed = true;
        // gameObject.SetActive(false);
    }

    public void Reset() {
        SetIndex(0);
        IsClosed = false;
    }

    private void Update() {
        GetComponent<CanvasGroup>().alpha = 
            IsClosed ? 0 : 1;
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapInfoUI : MonoBehaviour {
    public MapInfo mapInfo;
    public Image inkSlider;
    public TextMeshProUGUI inkText;
    public Image enemySlider;
    public TextMeshProUGUI enemyText;

    public InkManager inkManager;

    private void Start() {
        if (!mapInfo) {
            Debug.Log("No map loaded.");
            return;
        }

        inkSlider.fillAmount = mapInfo.startingInkFraction;
        int startingInk = (int)(mapInfo.startingInkFraction * mapInfo.totalInk);
        inkText.text = $"{startingInk} / {mapInfo.totalInk}";
        enemySlider.fillAmount = 0f;
        int numWaves = 0;
        foreach (var chunk in mapInfo.chunkInfo) {
            numWaves += chunk.waves.Length;
        }
        enemyText.text = $"{mapInfo.chunkInfo.Length} Chunks\n{numWaves} Waves";
        
    }

    private void Update() {
        if (inkManager != null) {
            inkSlider.fillAmount = inkManager.InkFraction;
            inkText.text = inkManager.InkString;
        }
    }


}

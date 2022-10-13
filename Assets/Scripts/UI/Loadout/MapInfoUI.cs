using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapInfoUI : MonoBehaviour {
    public MapInfo mapInfo;
    public Image inkSlider;
    public TextMeshProUGUI inkText;
    public Image enemySlider;
    public TextMeshProUGUI enemyText;

    private void Start() {
        if (!mapInfo) {
            Debug.Log("No map loaded.");
            return;
        }

        inkSlider.fillAmount = mapInfo.startingInkFraction;
        inkText.text = $"{mapInfo.startingInkFraction}";
        enemySlider.fillAmount = 0f;
        int numWaves = 0;
        foreach (var chunk in mapInfo.levelInfo) {
            numWaves += chunk.waves.Length;
        }
        enemyText.text = $"{mapInfo.levelInfo.Length} Chunks\n{numWaves} Waves";
        
    }

}

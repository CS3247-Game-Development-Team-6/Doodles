using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerDescriptionUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private TextMeshProUGUI towerDesc;
    [SerializeField] private Image towerImage;
    public int maxSlots;
    public TowerInfo towerInfo;
    public Sprite sprite;

    private void Update() {
        if (towerInfo == null) {
            towerName.text = "Towers";
            towerDesc.text = $"Hover over a tower to see its description.\n\nYou can choose up to {maxSlots} towers.";
            towerImage.enabled = false;
            return;
        }
        towerName.text = towerInfo.towerName;
        towerDesc.text = towerInfo.towerDesc;
        towerImage.sprite = sprite;
        towerImage.enabled = true;
    }

    public void SetInfo(TowerInfo towerInfo, Sprite sprite) {
        this.towerInfo = towerInfo;
        this.sprite = sprite;
    }

    public void ResetInfo() {
        towerInfo = null;
        sprite = null;
    }
}

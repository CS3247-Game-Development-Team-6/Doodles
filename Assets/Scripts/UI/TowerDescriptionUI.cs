using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerDescriptionUI : MonoBehaviour {
    [Header("Tower Descriptions")]
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private TextMeshProUGUI towerDesc;
    [SerializeField] private Image towerImage;

    [Header("Tower Attributes")]
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private TextMeshProUGUI range;
    [SerializeField] private TextMeshProUGUI attack;
    [SerializeField] private TextMeshProUGUI hp;

    [Header("Current Tower (Do not edit)")]
    public int maxSlots; 
    public TowerInfo towerInfo;
    public Sprite sprite;

    private TowerInfo lastClicked;

    private void Update() {
        if (towerInfo == null) {
            towerName.text = "View towers";
            towerDesc.text = $"You may choose up to {maxSlots} towers.";
            towerImage.enabled = false;
            return;
        }
        towerName.text = towerInfo.towerName;
        towerDesc.text = towerInfo.towerDesc;
        cost.text = towerInfo.cost.ToString();
        range.text = towerInfo.range.ToString();
        attack.text = towerInfo.damage.ToString();
        hp.text = towerInfo.health.ToString();
        towerImage.sprite = sprite;
        towerImage.enabled = true;
    }

    public void SetInfo(TowerInfo towerInfo, Sprite sprite) {
        this.towerInfo = towerInfo;
        this.sprite = sprite;
    }

    public void SelectInfo(TowerInfo towerInfo) {
        this.lastClicked = towerInfo;
    }

    public void ResetInfo() {
        if (!lastClicked) {
            towerInfo = null;
            sprite = null;
        } else {
            towerInfo = lastClicked;
            sprite = lastClicked.sprite;
        }
    }
}

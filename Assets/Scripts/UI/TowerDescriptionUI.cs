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
    private bool isBase = true;

    private void Update() {
        if (towerInfo == null) {
            towerName.text = "View towers";
            towerDesc.text = $"You may choose up to {maxSlots} towers.";
            towerImage.enabled = false;
            cost.text = range.text = attack.text = hp.text = "";
            return;
        }
        towerName.text = towerInfo.towerName;
        towerDesc.text = towerInfo.towerDesc;
        cost.text = isBase ? towerInfo.cost.ToString() : $"+{towerInfo.cost}";
        int damageDiff = !lastClicked ? 0 : towerInfo.damage - lastClicked.damage;
        float rangeDiff = !lastClicked ? 0 : towerInfo.range - lastClicked.range;

        range.text = isBase ? towerInfo.range.ToString() : $"{towerInfo.range} [{rangeDiff}]";
        attack.text = isBase ? towerInfo.damage.ToString() : $"{towerInfo.damage} [{damageDiff}]";
        hp.text = towerInfo.health.ToString();
        towerImage.sprite = sprite;
        towerImage.enabled = true;
    }

    public void SetInfo(TowerInfo towerInfo, Sprite sprite) {
        SetInfo(towerInfo, sprite, true);
    }

    public void SetInfo(TowerInfo towerInfo, Sprite sprite, bool isBase) {
        this.towerInfo = towerInfo;
        this.sprite = sprite;
        this.isBase = isBase;
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

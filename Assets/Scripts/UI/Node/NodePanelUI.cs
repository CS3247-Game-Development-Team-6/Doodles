using TMPro;
using UnityEngine;

public class NodePanelUI : MonoBehaviour {
    private RectTransform parentContainer;
    private CanvasGroup panelCanvas;
    private Transform player;
    private Node selectedNode;
    public TextMeshProUGUI healthUi;
    public TextMeshProUGUI dmgUi;
    public TextMeshProUGUI aoeUi;
    public TextMeshProUGUI speedUi;
    public TextMeshProUGUI rateUi;
    public TextMeshProUGUI rangeUi;
    public GameObject[] backgrounds;

    [SerializeField] private Color positiveIndicator;
    [SerializeField] private Color negativeIndicator;
    private Color defaultColor;

    private bool isCompare;
    private TowerInfo newTower;

    private void Start() {
        parentContainer = GetComponentInParent<RectTransform>();
        panelCanvas = GetComponent<CanvasGroup>();
        player = FindObjectOfType<PlayerMovement>().transform;
        Hide();
        defaultColor = healthUi.color;
    }

    private void FixedUpdate() {
        // Right clicking away when NodeUI is on now deactivates the NodeUI
        if (Input.GetMouseButtonDown(1)) {
            Hide();
        }
        if (!selectedNode || !selectedNode.tower) return;

        Tower tower = selectedNode.tower;

        if (isCompare) {
            CompareValues((int)tower.maxHealth, (int)newTower.health, healthUi);
            healthUi.text = $"{(int)tower.health} / {healthUi.text}";
            CompareValues(tower.towerInfo.damage, newTower.damage, dmgUi);
            CompareValues(tower.towerInfo.explosionRadius, newTower.explosionRadius, aoeUi);
            CompareValues(tower.towerInfo.speed, newTower.speed, speedUi);
            CompareValues(tower.towerInfo.fireRate, newTower.fireRate, rateUi);
            CompareValues(tower.towerInfo.range, newTower.range, rangeUi);

        } else {
            healthUi.text = $"{(int)tower.health} / {(int)tower.maxHealth}";
            dmgUi.text = $"{tower.towerInfo.damage}";
            aoeUi.text = $"{tower.towerInfo.explosionRadius}";
            speedUi.text = $"{tower.towerInfo.speed}";
            rateUi.text = $"{tower.towerInfo.fireRate}";
            rangeUi.text = $"{tower.towerInfo.range}";
            healthUi.color = dmgUi.color = aoeUi.color = speedUi.color = rateUi.color = rangeUi.color = defaultColor;
        }
    }

    public void SetTarget(Node target) {
        selectedNode = target;
        Tower tower = target.tower;

        if (!tower) {
            Debug.Log($"NodePanel: No tower selected from {target}");
            return;
        }

        NodeButtonUI[] nodeButtons = GetComponentsInChildren<NodeButtonUI>();
        foreach (NodeButtonUI button in nodeButtons) {
            button.SetTower(tower);
        }

        Unhide();
        parentContainer.position = target.transform.position + new Vector3(0, 2, 0);
    }

    private void CompareValues(float orig, float val, TextMeshProUGUI ui) {
        float diff = val - orig;
        if (diff == 0) { 
            ui.text = $"{val}";
        } else { 
            ui.text = $"{val} [" + (diff < 0 ? "" : "+") + $"{diff}]";
            ui.color = diff < 0 ? negativeIndicator : positiveIndicator;
        }

    }

    public void CompareTower(TowerInfo newTower) {
        if (!newTower || !selectedNode || !selectedNode.tower) {
            Debug.Log("NodePanel: No tower selected");
            return;
        }

        Tower tower = selectedNode.tower;
        this.newTower = newTower;
        isCompare = true;
    }

    public void ResetTowerInfo() {
        if (selectedNode) {
            isCompare = false;
            newTower = null;
        }
    }

    public void Hide() {
        foreach (var bg in backgrounds) {
            // bg.LeanMoveLocalY(-100, 0.9f).setOnComplete(() => bg.SetActive(false));
            bg.SetActive(false);
            Debug.Log("Hiding");
        }
        panelCanvas.LeanAlpha(0, 0.5f);
    }

    public void Unhide() {
        foreach (var bg in backgrounds) {
            bg.SetActive(true);
            // bg.LeanMoveLocalY(150, 0.9f).setEasePunch().setOnComplete(() => bg.SetActive(true));
            // bg.SetActive(true);
            Debug.Log("UnHiding");
        }
        panelCanvas.LeanAlpha(1, 0.5f);
    }
}

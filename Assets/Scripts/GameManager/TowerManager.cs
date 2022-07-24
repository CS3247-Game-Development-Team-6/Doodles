using UnityEngine;
using TMPro;

public class TowerManager : MonoBehaviour {
    public static TowerManager instance { get; private set; }
    private InkManager inkManager;
    private TowerInfo towerToBuild;
    private Node selectedNode;
    private TMP_Text actionTimer;

    private void Awake() {
        if (instance != null) {
            Debug.Log("TowerManager should be a singleton! Only 1 should exist in a scene.");
            return;
        }
        instance = this;

        // initialize action timer text
        actionTimer = GameObject.Find("ActionTimer").GetComponent<TMP_Text>();
        actionTimer.text = "";
        inkManager = GameObject.FindObjectOfType<InkManager>().GetComponent<InkManager>();
    }

    /** TODO: Fill in for Tooltip system */
    public void SelectNode(Node node) {
        if (selectedNode == node) {
            DeselectNode();
            return;
        }
        selectedNode = node;
        node.OpenTowerUpgrades();
        // nodeUI.SetTarget(node);
        /* if (node.tower != null) {
            UpdateUiTooltip(node);
        } */
    }

    /** TODO: Fill in for Tooltip system */
    public void DeselectNode() { 
        selectedNode = null;
        // nodeUI.Hide();
    }

    /** Returns cost of currently selected tower.
     */
    public int GetTowerCost() {
        if (!towerToBuild) {
            Debug.LogError("No tower type selected");
            return 0;
        }

        return towerToBuild.cost;
    }

    /** For building on a new tile on selected node. */
     public void BuildTower(Node node) {
        if (!towerToBuild) {
            Debug.LogError("No tower type selected");
            return;
        } else if (!towerToBuild.towerPrefab) {
            Debug.LogError("No tower prefab present");
            return;
        }
        int cost = towerToBuild.cost;

        Tower tower = node.BuildTower(towerToBuild);
        if (tower != null) {
            inkManager.ChangeInkAmount(-cost);
        } else {
            Debug.LogError("Tower not built by Node " + node);
        }

        DeselectNode();
    }

    /** For element changes on selected node. */
    public void ReplaceElementTower(ElementInfo element) {
        Debug.Log("Selected node", selectedNode);
        Debug.Log("Selected node tower", selectedNode.towerObj);
        Tower selectedTower = selectedNode.towerObj.GetComponent<Tower>();
        foreach (var pair in selectedTower.nextElements) {
            if (pair.element == element) {
                selectedNode.ReplaceTower(towerToBuild);
                break;
            }
        }
        DeselectNode();
    }

    /** For upgrades on selected node. */
    public void UpgradeTower() {
        Tower selectedTower = selectedNode.towerObj.GetComponent<Tower>();
        selectedNode.ReplaceTower(selectedTower.nextUpgrade);
        DeselectNode();
    }

    /** Destroys tower on selected node. */
    public void DestroyTower() {
        selectedNode.DestroyTower();
        DeselectNode();
    }

    public void SetTowerToBuild(TowerInfo towerInfo) {
        this.towerToBuild = towerInfo;
    }
}

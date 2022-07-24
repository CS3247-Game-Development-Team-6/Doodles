using UnityEngine;
using TMPro;

public class TowerManager : MonoBehaviour {
    public static TowerManager instance { get; private set; }
    [SerializeField] private ParticleSystem insufficientInkEffect;
    [SerializeField] private GameObject playerObj;
    private InkManager inkManager;
    private TowerInfo towerToBuild;
    private Node selectedNode;
    private TMP_Text actionTimer;
    private NodeUI nodeUI;

    private void Awake() {
        if (instance != null) {
            Debug.Log("TowerManager should be a singleton! Only 1 should exist in a scene.");
            return;
        }
        instance = this;

        // initialize action timer text
        actionTimer = GameObject.Find("ActionTimer").GetComponent<TMP_Text>();
        actionTimer.text = "";
        inkManager = InkManager.instance;
        nodeUI = GameObject.FindObjectOfType<NodeUI>().GetComponent<NodeUI>();
    }

    /** TODO: Fill in for Tooltip system */
    public void SelectNode(Node node) {
        if (selectedNode == node) {
            DeselectNode();
            return;
        }
        selectedNode = node;
        nodeUI.SetTarget(node);
    }

    /** TODO: Fill in for Tooltip system */
    public void DeselectNode() { 
        selectedNode = null;
        nodeUI.Hide();
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
        if (!inkManager.hasEnoughInk(cost)) {
            TriggerInsufficientInk();
        } else if (node.BuildTower(towerToBuild)) {
            inkManager.ChangeInkAmount(-cost);
        } else {
            Debug.LogError("Tower not built by Node " + node);
        }

        DeselectNode();
    }

    /** For element changes on selected node. */
    public void ReplaceElementTower(ElementInfo element) {
        Tower selectedTower = selectedNode.towerObj.GetComponent<Tower>();
        foreach (var pair in selectedTower.nextElements) {
            if (pair.element == element.type) {
                if (!inkManager.hasEnoughInk(pair.tower.cost)) {
                    TriggerInsufficientInk();
                } else if (selectedNode.ReplaceTower(pair.tower)) {
                    inkManager.ChangeInkAmount(-pair.tower.cost);
                } else {
                    Debug.LogError("Tower not built at Node " + selectedNode);
                }
                break;
            }
        }
        DeselectNode();
    }

    /** For upgrades on selected node. */
    public void UpgradeTower() {
        Tower selectedTower = selectedNode.towerObj.GetComponent<Tower>();
        if (!selectedTower.nextUpgrade) {
            Debug.LogError("No more upgrades for Node" + selectedNode);
            return;
        } else if (!inkManager.hasEnoughInk(selectedTower.nextUpgrade.cost)) {
            TriggerInsufficientInk();
        } else if (selectedNode.ReplaceTower(selectedTower.nextUpgrade)) {
            inkManager.ChangeInkAmount(-selectedTower.nextUpgrade.cost);
            selectedNode.SetIsUpgraded(true);
        } else {
            Debug.LogError("Tower not built at Node " + selectedNode);
        }
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

    public void TriggerInsufficientInk() {
        Instantiate(insufficientInkEffect, playerObj.transform.position, Quaternion.identity);
    }
}
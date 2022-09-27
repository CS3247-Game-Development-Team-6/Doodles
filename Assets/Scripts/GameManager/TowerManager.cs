using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerManager : MonoBehaviour {
    public static TowerManager instance { get; private set; }
    [SerializeField] private ParticleSystem insufficientInkEffect;
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private GameObject smokeEffectPrefab;
    [SerializeField] private GameObject soundEffectPrefab;
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

    public GameObject GetSmokeEffectPrefab() {
        return smokeEffectPrefab;
    }

    public GameObject GetSoundEffectPrefab() {
        return soundEffectPrefab;
    }

    public GameObject GetHealthBarPrefab() {
        return healthBarPrefab;
    }
    
    public bool CanBuildTower(Node node) {
        if (towerToBuild == null) {
            Debug.LogWarning($"Have not selected any tower");
            return false;
        }

        CellType nodeCellType = node.cell.type;
        List<CellType> allowedCellType = towerToBuild.allowedCellTypes;
        if (allowedCellType == null) towerToBuild.allowedCellTypes = new List<CellType>();
        if (allowedCellType.Count == 0) {
            towerToBuild.allowedCellTypes.Add(CellType.NONE);
            Debug.LogWarning($"No cell types allowed for {towerToBuild.name}. Adding NONE to the ScriptableObject.");
            return false;
        }

        int match = allowedCellType.FindIndex(type => type == nodeCellType);
        return match >= 0;
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

        if (!CanBuildTower(node)) return;

        int cost = towerToBuild.cost;
        InkManager inkManager = InkManager.instance;
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
        InkManager inkManager = InkManager.instance;
        TowerInfo elementTower = selectedTower.nextElement[element.type];
        if (!elementTower) {
            Debug.LogError("Element Tower not found");
            return;
        }

        if (!inkManager.hasEnoughInk(elementTower.cost)) {
            TriggerInsufficientInk();
        } else if (selectedNode.ReplaceTower(elementTower)) {
            inkManager.ChangeInkAmount(-elementTower.cost);
        } else {
            Debug.LogError("Tower not built at Node " + selectedNode);
        }

        /*
        foreach (var pair in selectedTower.nextElements) {
            if (pair.element == element.type) {
                Debug.Log(pair.element + " " + pair.tower.towerName);
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
        */
        DeselectNode();
    }

    /** For upgrades on selected node. */
    public void UpgradeTower() {
        Tower selectedTower = selectedNode.towerObj.GetComponent<Tower>();
        InkManager inkManager = InkManager.instance;
        if (!selectedTower.nextUpgrade) {
            Debug.LogError("No more upgrades for Node" + selectedNode);
            return;
        } else if (!inkManager.hasEnoughInk(selectedTower.nextUpgrade.cost)) {
            TriggerInsufficientInk();
        } else if (selectedNode.ReplaceTower(selectedTower.nextUpgrade)) {
            inkManager.ChangeInkAmount(-selectedTower.nextUpgrade.cost);
            selectedNode.isUpgraded = true;
        } else {
            Debug.LogError("Tower not built at Node " + selectedNode);
        }
        DeselectNode();
    }

    /** Fix tower on selected node. */
    public void FixTower() {
        Tower selectedTower = selectedNode.towerObj.GetComponent<Tower>();
        InkManager inkManager = InkManager.instance;
        if (!inkManager.hasEnoughInk(selectedTower.towerInfo.damageFixCost)) {
            TriggerInsufficientInk();
        } else if (selectedTower.RestoreHealth()) {
            inkManager.ChangeInkAmount(-selectedTower.towerInfo.damageFixCost);
        } else {
            Debug.LogError("Tower not fixed at Node " + selectedNode);
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

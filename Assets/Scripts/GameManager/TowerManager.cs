using System.Collections;
using System.Collections.Generic;
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

    public void SelectNode(Node node) {
        if (selectedNode == node) {
            DeselectNode();
            return;
        }
        selectedNode = node;
        // nodeUI.SetTarget(node);
        /* if (node.tower != null) {
            UpdateUiTooltip(node);
        } */
    }

    public void DeselectNode() { 
        selectedNode = null;
        // nodeUI.Hide();
    }

    public int GetTowerCost() {
        if (!towerToBuild) {
            Debug.LogError("No tower type selected");
            return -1;
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

        // TODO: Change to delete here.
        // Should NOT pass responsibility of creating/destroying tower to NodeUI!
        // nodeUI.target.DestroyTower();
        // nodeUI.target.SwapTower();

        // UPDATE UI TOOLTIP HERE

        DeselectNode();
    }

    /** For upgrades/element changes on selected node. */
    public void ReplaceTower(TowerInfo towerInfo) {
        // nodeUI.target.SetIsUpgraded(towerInfo.versionNum > 0);
        // nodeUI.target.SetIsAddedElement(towerInfo.element != null);

    }

    /** Destroys tower on selected node. */
    public void DestroyTower() {

    }

    public void SetTowerToBuild(TowerInfo towerInfo) {
        this.towerToBuild = towerInfo;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour {
    public static TowerManager instance;
    private TowerInfo towerToBuild;
    private Node selectedNode;
    private GameObject playerObj;
    private GameObject nodeUIObj;
    private NodeUI nodeUI;

    private void Awake() {
        if (instance != null) {
            Debug.Log("TowerManager should be a singleton! Only 1 should exist in a scene.");
            return;
        }
        instance = this;
    }

    public void SelectNode(Node node) {
        if (selectedNode == node) {
            DeselectNode();
            return;
        }
        selectedNode = node;
        nodeUI.SetTarget(node);
        /* if (node.tower != null) {
            UpdateUiTooltip(node);
        } */
    }

    public void DeselectNode() { 
        selectedNode = null;
        nodeUI.Hide();
    }

    /** For building on a new tile on selected node. */
    public void BuildTower() {
        if (!towerToBuild) return;
        int cost = towerToBuild.cost;
        Player player = playerObj.GetComponent<Player>();

        // TODO: Change to delete here.
        // Should NOT pass responsibility of creating/destroying tower to NodeUI!
        nodeUI.target.DestroyTower();
        nodeUI.target.SwapTower();

        player.ChangeInkAmount(-cost);
        // UPDATE UI TOOLTIP HERE

        DeselectNode();
    }

    /** For upgrades/element changes on selected node. */
    public void ReplaceTower(TowerInfo towerInfo) {
        nodeUI.target.SetIsUpgraded(towerInfo.versionNum > 0);
        nodeUI.target.SetIsAddedElement(towerInfo.element != null);

    }

    /** Destroys tower on selected node. */
    public void DestroyTower() {

    }

    public void SetTowerToBuild(TowerInfo towerInfo) {
        this.towerToBuild = towerInfo;
    }
}

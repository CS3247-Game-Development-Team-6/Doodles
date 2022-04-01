using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    private GameObject towerToBuild;
    private Node selectedNode;
    public NodeUI nodeUI;
    private GameObject currentTowerType;
    public GameObject standardTowerPrefab;
    public GameObject missileLauncherPrefab;
    public GameObject fireTurret;
    public GameObject iceTurret;
    public GameObject waterTurret;
    public GameObject fireMissileLauncher;
    public GameObject iceMissileLauncher;
    public GameObject waterMissileLauncher;

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.Log("This should be a singleton! Only 1 should exist in a scene.");
            return;
        }
        instance = this;
        towerToBuild = standardTowerPrefab;
        currentTowerType = towerToBuild;
    }

    public GameObject GetTowerToBuild()
    { 
        return towerToBuild;
    }

    public void SelectNode(Node node) 
    {
        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }
        Debug.Log("User has selected this tower");
        selectedNode = node;
        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    { 
        selectedNode = null;
        nodeUI.Hide();
    }

    public void SetTowerToBuild(GameObject tower)
    {
        towerToBuild = tower; 
        DeselectNode();
    }

    public GameObject GetCurrentTowerType()
    {
        return currentTowerType;
    }

    public void SetCurrentTowerType(GameObject tower)
    {
        currentTowerType = tower;
    }

    // Update is called once per frame
    void Update()
    {
        HandleTurretChange();
    }

    private void HandleTurretChange()
    {
        // For Testing Purposes (May be removed in the future)
        if (GetCurrentTowerType() == standardTowerPrefab)
        {
            // Debug.Log("in normal turret branch");
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                SetTowerToBuild(standardTowerPrefab);
            } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                SetTowerToBuild(fireTurret);
            } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                SetTowerToBuild(iceTurret);
            } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
                SetTowerToBuild(waterTurret);
            }
        }
        else if (GetCurrentTowerType() == missileLauncherPrefab)
        {
            // Debug.Log("in missile launcher branch");
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                SetTowerToBuild(missileLauncherPrefab);
            } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                SetTowerToBuild(fireMissileLauncher);
            } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                SetTowerToBuild(iceMissileLauncher);
            } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
                SetTowerToBuild(waterMissileLauncher);
            }
        }
    }

    public void setFireTurret()
    {
        if (currentTowerType == standardTowerPrefab)
        {
            SetTowerToBuild(fireTurret);
        }
        else if (currentTowerType == missileLauncherPrefab)
        {
            SetTowerToBuild(fireMissileLauncher);
        }
    }

    public void setIceTurret()
    {
        if (currentTowerType == standardTowerPrefab)
        {
            SetTowerToBuild(iceTurret);
        }
        else if (currentTowerType == missileLauncherPrefab)
        {
            SetTowerToBuild(iceMissileLauncher);
        }
    }

    public void setWaterTurret()
    {
        if (currentTowerType == standardTowerPrefab)
        {
            SetTowerToBuild(waterTurret);
        }
        else if (currentTowerType == missileLauncherPrefab)
        {
            SetTowerToBuild(waterMissileLauncher);
        }
    }

    public void DestroyTower()
    {
        Debug.Log("Im here!");
        if (selectedNode.GetTower() == null)
        {
            Debug.Log("There's no tower in the node!");
        }
        selectedNode.DestroyTower();
        selectedNode.setIsTowerBuilt(false);
        DeselectNode();
    }
}

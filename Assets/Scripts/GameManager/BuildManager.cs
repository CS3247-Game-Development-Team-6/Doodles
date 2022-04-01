using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    private GameObject towerToBuild;
    public GameObject playerGO;
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
    public GameObject upTowerPrefab;
    public GameObject upMissileLauncherPrefab;
    public GameObject upFireTurret;
    public GameObject upIceTurret;
    public GameObject upWaterTurret;
    public GameObject upFireMissileLauncher;
    public GameObject upIceMissileLauncher;
    public GameObject upWaterMissileLauncher;


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

    // Change tower elements
    public void SwapTower(GameObject towerToBuild, GameObject playerGO)
    {
        float cost = towerToBuild.GetComponent<Turret>().GetSwapElementCost();
        Player player = playerGO.GetComponent<Player>();
        // Check if enough ink
        if (!player.hasEnoughInk(cost))
        {
            return;
        }

        // Get rid of original tower
        selectedNode.DestroyTower();

        // Build new tower with correct element
        selectedNode.SwapTower();

        player.ChangeInkAmount(-cost);

        selectedNode.SetIsAddedElement(true);

        // Reset towerToBuild
        if (towerToBuild.GetComponent<Turret>().name.Contains("Turret"))
            this.towerToBuild = standardTowerPrefab;
        else if (towerToBuild.GetComponent<Turret>().name.Contains("MissileLauncher"))
            this.towerToBuild = missileLauncherPrefab;

        // Hide nodeUI
        DeselectNode();
    }

    public void buildFireTurret()
    {

        // Check if already added element
        if (selectedNode.GetIsAddedElement())
        {
            return;
        }

        // Set respective towers
        if (!selectedNode.GetIsUpgraded())
        {
            if (selectedNode.tower.tag == "Turret")
            {
                towerToBuild = fireTurret;
            }
            else if (selectedNode.tower.tag == "Missile")
            {
                towerToBuild = fireMissileLauncher;
            }
        }
        else
        {
            if (selectedNode.tower.tag == "Turret")
            {
                towerToBuild = upFireTurret;
            }
            else if (selectedNode.tower.tag == "Missile")
            {
                towerToBuild = upFireMissileLauncher;
            }
        }

        SwapTower(this.towerToBuild, this.playerGO);
    }

    public void buildIceTurret()
    {
        // Check if already added element
        if (selectedNode.GetIsAddedElement())
        {
            return;
        }

        // Set respective towers
        if (!selectedNode.GetIsUpgraded())
        {
            if (selectedNode.tower.tag == "Turret")
            {
                towerToBuild = iceTurret;
            }
            else if (selectedNode.tower.tag == "Missile")
            {
                towerToBuild = iceMissileLauncher;
            }
        }
        else
        {
            if (selectedNode.tower.tag == "Turret")
            {
                towerToBuild = upIceTurret;
            }
            else if (selectedNode.tower.tag == "Missile")
            {
                towerToBuild = upIceMissileLauncher;
            }
        }

        SwapTower(this.towerToBuild, this.playerGO);
    }

    public void buildWaterTurret()
    {
        // Check if already added element
        if (selectedNode.GetIsAddedElement())
        {
            return;
        }

        // Set respective towers
        if (!selectedNode.GetIsUpgraded())
        {
            if (selectedNode.tower.tag == "Turret")
            {
                towerToBuild = waterTurret;
            }
            else if (selectedNode.tower.tag == "Missile")
            {
                towerToBuild = waterMissileLauncher;
            }
        }
        else
        { 
            if (selectedNode.tower.tag == "Turret")
            {
                towerToBuild = upWaterTurret;
            } 
            else if (selectedNode.tower.tag == "Missile")
            {
                towerToBuild = upWaterMissileLauncher;
            }
        }

        SwapTower(this.towerToBuild, this.playerGO);
    }

    public void UpgradeTower()
    { 
        if (selectedNode.GetIsUpgraded())
        {
            return;
        }

        if (selectedNode.tower.tag == "Turret")
        {
            if (selectedNode.tower.GetComponent<Turret>().bulletPrefab.tag == "Basic")
            {
                towerToBuild = upTowerPrefab;
            }

            else if (selectedNode.tower.GetComponent<Turret>().bulletPrefab.tag == "Fire")
            {
                towerToBuild = upFireTurret;
            }

            else if (selectedNode.tower.GetComponent<Turret>().bulletPrefab.tag == "Ice")
            {
                towerToBuild = upIceTurret;
            }

            else if (selectedNode.tower.GetComponent<Turret>().bulletPrefab.tag == "Water")
            {
                towerToBuild = upWaterTurret;
            }
        }

        else if (selectedNode.tower.tag == "Missile")
        {
            if (selectedNode.tower.GetComponent<Turret>().bulletPrefab.tag == "Basic")
            {
                towerToBuild = upMissileLauncherPrefab;
            }

            else if (selectedNode.tower.GetComponent<Turret>().bulletPrefab.tag == "Fire")
            {
                towerToBuild = upFireMissileLauncher;
            }

            else if (selectedNode.tower.GetComponent<Turret>().bulletPrefab.tag == "Ice")
            {
                towerToBuild = upIceMissileLauncher;
            }

            else if (selectedNode.tower.GetComponent<Turret>().bulletPrefab.tag == "Water")
            {
                towerToBuild = upWaterMissileLauncher;
            }
        }

        SwapTower(this.towerToBuild, this.playerGO);
    }

    

    public void DestroyTower()
    {
        selectedNode.DestroyTower();
        selectedNode.SetIsTowerBuilt(false);
        selectedNode.SetIsAddedElement(false);
        // Hide nodeUI
        DeselectNode();
    }
}

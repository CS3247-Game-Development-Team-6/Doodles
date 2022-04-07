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
    public GameObject standardTowerPrefab;
    public GameObject missileLauncherPrefab;
    public GameObject aoeTowerPrefab;
    public GameObject fireTurret;
    public GameObject iceTurret;
    public GameObject waterTurret;
    public GameObject fireMissileLauncher;
    public GameObject iceMissileLauncher;
    public GameObject waterMissileLauncher;
    public GameObject fireAoeTower;
    public GameObject iceAoeTower;
    public GameObject waterAoeTower;
    public GameObject upTowerPrefab;
    public GameObject upMissileLauncherPrefab;
    public GameObject upFireTurret;
    public GameObject upIceTurret;
    public GameObject upWaterTurret;
    public GameObject upFireMissileLauncher;
    public GameObject upIceMissileLauncher;
    public GameObject upWaterMissileLauncher;
    public GameObject upAoeTower;
    public GameObject upFireAoeTower;
    public GameObject upIceAoeTower;
    public GameObject upWaterAoeTower;


    private void Awake()
    {
        if (instance != null) 
        {
            Debug.Log("This should be a singleton! Only 1 should exist in a scene.");
            return;
        }
        instance = this;
        towerToBuild = standardTowerPrefab;
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
        Debug.Log(tower.name);
        towerToBuild = tower; 
        DeselectNode();
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
        nodeUI.target.DestroyTower();

        // Build new tower with correct element
        nodeUI.target.SwapTower();

        player.ChangeInkAmount(-cost);

        // Reset towerToBuild
        if (towerToBuild.tag == "Turret")
            this.towerToBuild = standardTowerPrefab;
        else if (towerToBuild.tag == "Missile")
            this.towerToBuild = missileLauncherPrefab;
        else if (towerToBuild.tag == "AOE")
            this.towerToBuild = aoeTowerPrefab;

        // Hide nodeUI
        DeselectNode();
    }

    public void buildFireTurret()
    {
/*        Debug.Log("This cell has element: " + nodeUI.target.GetIsAddedElement());*/

        // Check if already added element
        if (nodeUI.target.GetIsAddedElement())
        {
            return;
        }

/*        Debug.Log("This cell has upgrade: " + nodeUI.target.GetIsUpgraded());*/

        // Set respective towers
        if (!nodeUI.target.GetIsUpgraded())
        {
            if (nodeUI.target.tower.tag == "Turret")
            {
                towerToBuild = fireTurret;
            }
            else if (nodeUI.target.tower.tag == "Missile")
            {
                towerToBuild = fireMissileLauncher;
            }
            else if (nodeUI.target.tower.tag == "AOE")
            {
                towerToBuild = fireAoeTower;
            }
        }
        else
        {
            if (nodeUI.target.tower.tag == "Turret")
            {
                towerToBuild = upFireTurret;
            }
            else if (nodeUI.target.tower.tag == "Missile")
            {
                towerToBuild = upFireMissileLauncher;
            }
            else if (nodeUI.target.tower.tag == "AOE")
            {
                towerToBuild = upFireAoeTower;
            }
        }

        if (!playerGO.GetComponent<Player>().hasEnoughInk(towerToBuild.GetComponent<Turret>().GetSwapElementCost()))
        {
/*            Debug.Log("No cash no upgrade");*/
            return;
        }

        SwapTower(this.towerToBuild, this.playerGO);
        nodeUI.target.SetIsAddedElement(true);
    }

    public void buildIceTurret()
    {
/*        Debug.Log("This cell has element: " + nodeUI.target.GetIsAddedElement());*/

        // Check if already added element
        if (nodeUI.target.GetIsAddedElement())
        {
            return;
        }

/*        Debug.Log("This cell has upgrade: " + nodeUI.target.GetIsUpgraded());*/

        // Set respective towers
        if (!nodeUI.target.GetIsUpgraded())
        {
            if (nodeUI.target.tower.tag == "Turret")
            {
                towerToBuild = iceTurret;
            }
            else if (nodeUI.target.tower.tag == "Missile")
            {
                towerToBuild = iceMissileLauncher;
            }
            else if (nodeUI.target.tower.tag == "AOE")
            {
                towerToBuild = iceAoeTower;
            }
        }
        else
        {
            if (nodeUI.target.tower.tag == "Turret")
            {
                towerToBuild = upIceTurret;
            }
            else if (nodeUI.target.tower.tag == "Missile")
            {
                towerToBuild = upIceMissileLauncher;
            }
            else if (nodeUI.target.tower.tag == "AOE")
            {
                towerToBuild = upIceAoeTower;
            }
        }

        if (!playerGO.GetComponent<Player>().hasEnoughInk(towerToBuild.GetComponent<Turret>().GetSwapElementCost()))
        {
/*            Debug.Log("No cash no upgrade");*/
            return;
        }

        SwapTower(this.towerToBuild, this.playerGO);
        nodeUI.target.SetIsAddedElement(true);
    }

    public void buildWaterTurret()
    {
/*        Debug.Log("This cell has element: " + nodeUI.target.GetIsAddedElement());*/

        // Check if already added element
        if (nodeUI.target.GetIsAddedElement())
        {
            return;
        }

/*        Debug.Log("This cell has upgrade: " + nodeUI.target.GetIsUpgraded());*/

        // Set respective towers
        if (!nodeUI.target.GetIsUpgraded())
        {
            if (nodeUI.target.tower.tag == "Turret")
            {
                towerToBuild = waterTurret;
            }
            else if (nodeUI.target.tower.tag == "Missile")
            {
                towerToBuild = waterMissileLauncher;
            }
            else if (nodeUI.target.tower.tag == "AOE")
            {
                towerToBuild = waterAoeTower;
            }
        }
        else
        { 
            if (nodeUI.target.tower.tag == "Turret")
            {
                towerToBuild = upWaterTurret;
            } 
            else if (nodeUI.target.tower.tag == "Missile")
            {
                towerToBuild = upWaterMissileLauncher;
            }
            else if (nodeUI.target.tower.tag == "AOE")
            {
                towerToBuild = upWaterAoeTower;
            }
        }

        if (!playerGO.GetComponent<Player>().hasEnoughInk(towerToBuild.GetComponent<Turret>().GetSwapElementCost()))
        {
/*            Debug.Log("No cash no upgrade");*/
            return;
        }

        SwapTower(this.towerToBuild, this.playerGO);
        nodeUI.target.SetIsAddedElement(true);
    }

    public void UpgradeTower()
    {
        if (nodeUI.target.GetIsUpgraded() && nodeUI.target.GetIsAddedElement())
        {
            return;
        }

        if (nodeUI.target.tower.tag == "Turret")
        {
            if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Basic")
            {
                towerToBuild = upTowerPrefab;
            }

            else if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Fire")
            {
                towerToBuild = upFireTurret;
            }

            else if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Ice")
            {
                towerToBuild = upIceTurret;
            }

            else if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Water")
            {
                towerToBuild = upWaterTurret;
            }
        }

        else if (nodeUI.target.tower.tag == "Missile")
        {
            if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Basic")
            {
                towerToBuild = upMissileLauncherPrefab;
            }

            else if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Fire")
            {
                towerToBuild = upFireMissileLauncher;
            }

            else if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Ice")
            {
                towerToBuild = upIceMissileLauncher;
            }

            else if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Water")
            {
                towerToBuild = upWaterMissileLauncher;
            }
        }

        else if (nodeUI.target.tower.tag == "AOE")
        {
            if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Basic")
            {
                towerToBuild = upAoeTower;
            }

            else if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Fire")
            {
                towerToBuild = upFireAoeTower;
            }

            else if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Ice")
            {
                towerToBuild = upIceAoeTower;
            }

            else if (nodeUI.target.tower.GetComponent<Turret>().bulletPrefab.tag == "Water")
            {
                towerToBuild = upWaterAoeTower;
            }
        }

        if (!playerGO.GetComponent<Player>().hasEnoughInk(towerToBuild.GetComponent<Turret>().GetSwapElementCost()))
        {
/*            Debug.Log("No cash no upgrade");*/
            return;
        }

        SwapTower(this.towerToBuild, this.playerGO);
        nodeUI.target.SetIsUpgraded(true);
    }

    

    public void DestroyTower()
    {
        nodeUI.target.DestroyTower();
        nodeUI.target.SetIsTowerBuilt(false);
        nodeUI.target.SetIsAddedElement(false);
        nodeUI.target.SetIsUpgraded(false);
        // Hide nodeUI
        DeselectNode();
    }
}

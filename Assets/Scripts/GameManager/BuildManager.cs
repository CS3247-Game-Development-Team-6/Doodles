using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    public GameObject defaultTower;
    private GameObject towerToBuild;
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
        towerToBuild = defaultTower;
    }

    public GameObject GetTowerToBuild()
    { 
        return towerToBuild;
    }

    public void SetTowerToBuild(GameObject tower)
    {
        towerToBuild = tower; 
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
            Debug.Log("in normal turret branch");
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetTowerToBuild(standardTowerPrefab);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetTowerToBuild(fireTurret);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetTowerToBuild(iceTurret);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SetTowerToBuild(waterTurret);
            }
        }
        else if (GetCurrentTowerType() == missileLauncherPrefab)
        {
            Debug.Log("in missile launcher branch");
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetTowerToBuild(missileLauncherPrefab);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetTowerToBuild(fireMissileLauncher);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetTowerToBuild(iceMissileLauncher);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SetTowerToBuild(waterMissileLauncher);
            }
        }
    } 
}

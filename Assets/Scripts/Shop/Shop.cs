using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void SetTurretToBuild() 
    {
        buildManager.SetTowerToBuild(buildManager.standardTowerPrefab);
    }

    public void SetMissileLauncher()
    {
        buildManager.SetTowerToBuild(buildManager.missileLauncherPrefab);
    }
}

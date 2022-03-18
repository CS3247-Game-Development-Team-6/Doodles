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

    public void SetTurrentToBuild() 
    {
        buildManager.SetTowerToBuild(buildManager.standardTowerPrefab);
    }

    public void SetTurrent2ToBuild()
    {
        buildManager.SetTowerToBuild(buildManager.standardTowerPrefab2);
    }
}

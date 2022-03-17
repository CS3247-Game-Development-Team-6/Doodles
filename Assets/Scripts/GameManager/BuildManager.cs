using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    private GameObject towerToBuild;
    public GameObject standardTowerPrefab;
    public GameObject fireTurret;
    public GameObject iceTurret;
    public GameObject waterTurret;

    // Start is called before the first frame update
    void Start()
    {
        towerToBuild = standardTowerPrefab;
    }

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.Log("This should be a singleton! Only 1 should exist in a scene.");
            return;
        }
        instance = this;
    }

    public GameObject GetTowerToBuild()
    { 
        return towerToBuild;
    }

    // Update is called once per frame
    void Update()
    {
        // For Testing Purposes (May be removed in the future)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            towerToBuild = standardTowerPrefab;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            towerToBuild = fireTurret;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            towerToBuild = iceTurret;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            towerToBuild = waterTurret;
        }
    }
}

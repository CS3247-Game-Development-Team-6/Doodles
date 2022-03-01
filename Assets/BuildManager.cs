using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    private GameObject towerToBuild;
    public GameObject standardTowerPrefab;

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
        
    }
}

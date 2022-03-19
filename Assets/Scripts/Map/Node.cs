using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    private Color startColor;
    public Color tooFarColor;
    private Renderer rend;

    private GameObject tower;

    public GameObject[] noneTileModels;

    private void Start()
    {
        rend= GetComponent<Renderer>();
        startColor =rend.material.color;

        GameObject prefab;
        if (noneTileModels.Length == 0) return;
        int indexChosen = (int) Random.Range(0, noneTileModels.Length);
        prefab = noneTileModels[indexChosen];
        // tileModel = Object.Instantiate(prefab, transform);

        GetComponent<MeshFilter>().sharedMesh = Instantiate(prefab, transform).GetComponent<MeshFilter>().sharedMesh;
    }

    private void OnMouseEnter()
    {
        rend.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    public float TowerCost() {
        GameObject towerToBuild = BuildManager.instance.GetTowerToBuild();
        return towerToBuild.GetComponent<Turret>().Cost;
    }

    public bool HasTower() {
        return tower != null;
    }

    public Turret BuildTower()
    {
        if (tower != null) 
        {
            Debug.Log("Tower cannot be built here! TODO: Show Prompt on screen");
            return null;
        }

        // build a tower
        GameObject towerToBuild = BuildManager.instance.GetTowerToBuild();
        tower = (GameObject) Instantiate(towerToBuild, transform.position, transform.rotation);
        return tower.GetComponent<Turret>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
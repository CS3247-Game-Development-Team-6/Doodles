using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    private Color startColor;
    public Color tooFarColor;

    private GameObject tower;

    public Vector3 tileOffset = Vector3.zero;
    public GameObject tileMesh;
    private Renderer tileRenderer;
    private GameObject decorationMesh;

    public GameObject[] noneTileModels;

    private void Start()
    {
        tileRenderer = tileMesh.GetComponent<Renderer>();
        startColor = tileRenderer.material.color;

        GameObject prefab;
        if (noneTileModels.Length == 0) return;
        int indexChosen = (int) Random.Range(0, noneTileModels.Length);
        prefab = noneTileModels[indexChosen];

        decorationMesh = Instantiate(prefab, transform.position + tileOffset, transform.rotation);
        decorationMesh.transform.SetParent(transform);
    }

    private void OnMouseEnter()
    {
        tileRenderer.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        tileRenderer.material.color = startColor;
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
        tower = (GameObject) Instantiate(towerToBuild, tileMesh.transform.position + tileOffset, tileMesh.transform.rotation);
        Destroy(decorationMesh);
        return tower.GetComponent<Turret>();
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    private Color startColor;
    public Color tooFarColor;

    private bool isTowerBuilt = false;
    private GameObject tower;
    public Cell cell;

    public Vector3 tileOffset = Vector3.zero;
    public Vector3 towerOffset = Vector3.zero;
    public GameObject tileMesh;
    private Renderer tileRenderer;
    private GameObject decorationMesh;

    public GameObject[] noneTileModels;
    private BuildManager buildManager;
    private PlayerMovement playerMovement;
    private GameObject playerObject;

    public Vector3 towerBuildPosition;

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
        
        buildManager = BuildManager.instance;
        playerObject = GameObject.Find("Player");
        playerMovement = playerObject.GetComponent<PlayerMovement>();
    }

    public bool hasTowerBuilt()
    {
        return isTowerBuilt;
    }

    public float TowerCost() {
        GameObject towerToBuild = BuildManager.instance.GetTowerToBuild();
        return towerToBuild.GetComponent<Turret>().Cost;
    }

    public bool HasTower() {
        buildManager.SelectNode(this);
        return tower != null;
    }

    public void ToggleObjectView(bool visible) {
        decorationMesh.SetActive(visible);
    }

    public Turret BuildTower()
    {

        if (buildManager.GetTowerToBuild() == null) 
        {
            Debug.Log("Tower cannot be built here! TODO: Show Prompt on screen");
            return null;
        }

        if (tower != null) 
        {
            Debug.Log("Tower cannot be built here! TODO: Show Prompt on screen");
            return null;
        }

        // build a tower
        GameObject towerToBuild = buildManager.GetTowerToBuild();
        towerBuildPosition = tileMesh.transform.position + towerOffset;
        isTowerBuilt = true;
        tower = (GameObject) Instantiate(towerToBuild, tileMesh.transform.position + towerOffset, tileMesh.transform.rotation);
        Destroy(decorationMesh);
        return tower.GetComponent<Turret>();
    }

    public Vector3 GetTowerBuildPosition()
    {
        return towerBuildPosition;
    }

    private void OnMouseOver() {
        if (EventSystem.current.IsPointerOverGameObject() || buildManager.GetTowerToBuild() == null) {
            return;
        }

        if ((transform.position - playerObject.transform.position).magnitude > playerMovement.GetBuildDistance()) {
            tileRenderer.material.color = tooFarColor;
        } else {
            tileRenderer.material.color = hoverColor;
        }
    }

    private void OnMouseExit() {
        tileRenderer.material.color = startColor;
    }

}
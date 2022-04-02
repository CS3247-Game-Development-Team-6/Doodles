using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class TileType {
    public GameObject tilePrefab;
    // the difference between freqMax and the 
    // previous element's freqMax is its actual frequency.
    [Range(0, 1)] public float frequencyMax;
}

public class Node : MonoBehaviour
{
    public Color hoverColor;
    private Color startColor;
    public Color tooFarColor;

    private bool isTowerBuilt = false;
    private bool isAddedElement = false;
    private GameObject tower;
    public Cell cell;

    public Vector3 tileOffset = Vector3.zero;
    public Vector3 towerOffset = Vector3.zero;
    public GameObject tileMesh;
    private Renderer tileRenderer;
    private GameObject decorationMesh;

    public GameObject defaultTile;
    public TileType[] tileTypes;
    private BuildManager buildManager;
    private PlayerMovement playerMovement;
    private GameObject playerObject;

    private Vector3 towerBuildPosition;

    private void Start()
    {
        tileRenderer = tileMesh.GetComponent<Renderer>();
        startColor = tileRenderer.material.color;

        GameObject prefab = defaultTile;
        
        if (tileTypes.Length > 1) {
            float freq = Random.Range(0f, 1f);
            prefab = tileTypes[0].tilePrefab;
            for (int i = 1; i < tileTypes.Length; i++) {
                if (tileTypes[i-1].frequencyMax < freq && tileTypes[i].frequencyMax >= freq) {
                    prefab = tileTypes[i].tilePrefab;
                    break;
                }
            }
        }

        decorationMesh = Instantiate(prefab, transform.position + tileOffset, transform.rotation);
        decorationMesh.transform.SetParent(transform);
        
        buildManager = BuildManager.instance;
        playerObject = GameObject.Find("Player");
        playerMovement = playerObject.GetComponent<PlayerMovement>();
    }

    public GameObject SetTower(GameObject tower)
    {
        this.tower = tower;
        return tower;
    }

    public GameObject GetTower()
    {
        return this.tower;
    }
    public void DestroyTower()
    {
        Destroy(this.tower);
        tower = null;
    }
    public bool GetIsTowerBuilt()
    {
        return isTowerBuilt;
    }

    public void SetIsTowerBuilt(bool b)
    {
        isTowerBuilt = b;
    }

    public bool GetIsAddedElement()
    {
        return isAddedElement;
    }

    public void SetIsAddedElement(bool b)
    {
        isAddedElement = b;
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
            return null;
        }

        if (tower != null) 
        {
            return null;
        }

        // build a tower
        GameObject towerToBuild = buildManager.GetTowerToBuild();
        towerBuildPosition = tileMesh.transform.position + towerOffset;
        SetIsTowerBuilt(true);
        tower = (GameObject) Instantiate(towerToBuild, tileMesh.transform.position + towerOffset, Quaternion.identity);
        Destroy(decorationMesh);    // Destroy current node's asset

        return tower.GetComponent<Turret>();
    }

    // Build swapped tower
    public void SwapTower()
    {
        if (buildManager.GetTowerToBuild() == null)
        {
            Debug.Log("There's no tower to build");
            return;
        }
        Debug.Log("I'm in node.cs SwapTower()");
        GameObject towerToBuild = buildManager.GetTowerToBuild();
        tower = (GameObject)Instantiate(towerToBuild, towerBuildPosition, Quaternion.identity);
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
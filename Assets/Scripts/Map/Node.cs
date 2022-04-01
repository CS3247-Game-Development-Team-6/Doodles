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
    public GameObject tower;
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

    public GameObject GetTower()
    {
        return tower;
    }
    public void DestroyTower()
    {
        Destroy(this.tower);
        tower = null;
    }
    public bool getIsTowerBuilt()
    {
        return isTowerBuilt;
    }

    public void setIsTowerBuilt(bool b)
    {
        isTowerBuilt = b;
    }

    public float TowerCost() {
        GameObject towerToBuild = BuildManager.instance.GetTowerToBuild();
        return towerToBuild.GetComponent<Turret>().Cost;
    }

    public float SwapTowerElementCost()
    {
        GameObject towerToBuild = BuildManager.instance.GetTowerToBuild();
        return towerToBuild.GetComponent<Turret>().GetSwapElementCost();
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
        setIsTowerBuilt(true);
        tower = (GameObject) Instantiate(towerToBuild, tileMesh.transform.position + towerOffset, Quaternion.identity);
        Destroy(decorationMesh);    // Destroy current node's asset
        return tower.GetComponent<Turret>();
    }

    public void UpgradeTurret()
    {
        if (tower == null)
        {
            Debug.Log("No tower is found at this cell");
            return;
        }

        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasEnoughInk(tower.GetComponent<Turret>().upgradeCost))
        {
            Debug.Log("Player not enough ink to upgrade");
            return;
        }
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
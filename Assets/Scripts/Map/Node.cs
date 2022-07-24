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
    private bool isUpgraded = false;
    public GameObject towerObj;
    public Cell cell;

    public Vector3 tileOffset = Vector3.zero;
    public Vector3 towerOffset = Vector3.zero;
    public GameObject tileMesh;
    private Renderer tileRenderer;
    private GameObject decorationMesh;

    public GameObject defaultTile;
    public TileType[] tileTypes;
    private BuildManager buildManager;

    private Vector3 towerBuildPosition;
    private PlayerMovement playerMovement;
    private GameObject playerObject;

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
        this.towerObj = tower;
        return tower;
    }

    public GameObject GetTower()
    {
        return this.towerObj;
    }
    public void DestroyTower()
    {
        Destroy(this.towerObj);
        towerObj = null;
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

    public bool GetIsUpgraded()
    {
        return isUpgraded;
    }

    public void SetIsUpgraded(bool b)
    { 
        isUpgraded = b;
    }

    public float TowerCost() {
        GameObject towerToBuild = BuildManager.instance.GetTowerToBuild();
        return towerToBuild.GetComponent<Turret>().Cost;
    }

    public void OpenTowerUpgrades() {
        buildManager.SelectNode(this);
    }

    public bool HasTower() {
        return towerObj != null;
    }

    public void ToggleObjectView(bool visible) {
        decorationMesh.SetActive(visible);
    }

    // DEPRECATING: Refer to BuildTower for newest version.
    public Turret BuildTower()
    {

        if (buildManager.GetTowerToBuild() == null) 
        {
            return null;
        }

        if (towerObj != null) 
        {
            return null;
        }

        // build a tower
        GameObject towerToBuild = buildManager.GetTowerToBuild();
        towerBuildPosition = tileMesh.transform.position + towerOffset;
        towerObj = (GameObject) Instantiate(towerToBuild, tileMesh.transform.position + towerOffset, Quaternion.identity);
        SetIsTowerBuilt(true);
        Destroy(decorationMesh);    // Destroy the flora on the tile.

        return towerObj.GetComponent<Turret>();
    }

    /** Removes decorations and creates new tower based on the towerInfo.  */
    public Tower BuildTower(TowerInfo towerInfo) {
        towerBuildPosition = tileMesh.transform.position + towerOffset;
        towerObj = Instantiate(towerInfo.towerPrefab, towerBuildPosition, Quaternion.identity);
        towerObj.GetComponent<Tower>().SetTowerInfo(towerInfo);
        SetIsTowerBuilt(true);
        Destroy(decorationMesh);    // Destroy the flora on the tile.

        return towerObj.GetComponent<Tower>();
    }

    // DEPRECATING: Refer to ReplaceTower for the newest version
    public void SwapTower() {
        if (buildManager.GetTowerToBuild() == null)
        {
            return;
        }
        GameObject towerToBuild = buildManager.GetTowerToBuild();
        towerObj = (GameObject)Instantiate(towerToBuild, towerBuildPosition, Quaternion.identity);
    }

    /** Replace existing tower upon upgrade. */
    public Tower ReplaceTower(TowerInfo towerInfo) {
        Destroy(towerObj);
        towerBuildPosition = tileMesh.transform.position + towerOffset;
        towerObj = Instantiate(towerInfo.towerPrefab, towerBuildPosition, Quaternion.identity);
        towerObj.GetComponent<Tower>().SetTowerInfo(towerInfo);
        SetIsTowerBuilt(true);

        return towerObj.GetComponent<Tower>();
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

        if (GetIsTowerBuilt()) {
            towerObj.gameObject.GetComponent<Outline>().enabled = true;
            towerObj.gameObject.GetComponent<Outline>().OutlineColor =
                (transform.position - playerObject.transform.position).magnitude > playerMovement.GetBuildDistance()
                    ? tooFarColor
                    : hoverColor;
        } else if (decorationMesh != null) {
            decorationMesh.gameObject.GetComponent<Outline>().enabled = true;
            decorationMesh.gameObject.GetComponent<Outline>().OutlineWidth = 5f;
            decorationMesh.gameObject.GetComponent<Outline>().OutlineColor = 
                (transform.position - playerObject.transform.position).magnitude > playerMovement.GetBuildDistance()
                    ? tooFarColor
                    : hoverColor;
        }
    }

    private void OnMouseExit() {
        tileRenderer.material.color = startColor;
        
        if (GetIsTowerBuilt()) {
            towerObj.gameObject.GetComponent<Outline>().enabled = false;
        } else if (decorationMesh != null) {
            decorationMesh.gameObject.GetComponent<Outline>().enabled = false;
        }
    }

}
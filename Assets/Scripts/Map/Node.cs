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

    public bool isUpgraded { get; set; }
    public GameObject towerObj;
    public Tower tower { get; private set; }
    public Cell cell;

    public Vector3 tileOffset = Vector3.zero;
    public Vector3 towerOffset = Vector3.zero;
    public GameObject tileMesh;
    private Renderer tileRenderer;
    private GameObject decorationMesh;

    public GameObject defaultTile;
    public TileType[] tileTypes;

    public Vector3 towerBuildPosition { get; private set; }
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

        if (prefab != null) {
            decorationMesh = Instantiate(prefab, transform.position + tileOffset, transform.rotation);
            decorationMesh.transform.SetParent(transform);
        }
        
        playerObject = GameObject.Find("Player");
        playerMovement = playerObject.GetComponent<PlayerMovement>();
    }

    public void DestroyTower() {
        Destroy(this.towerObj);
        tower = null;
        towerObj = null;
    }


    public bool HasTower() {
        return towerObj != null;
    }

    public void ToggleObjectView(bool visible) {
        decorationMesh.SetActive(visible);
    }

    /** Removes decorations and creates new tower based on the towerInfo.  */
    public Tower BuildTower(TowerInfo towerInfo) {
        towerBuildPosition = tileMesh.transform.position + towerOffset;
        towerObj = Instantiate(towerInfo.towerPrefab, towerBuildPosition, Quaternion.identity);
        tower = towerObj.GetComponent<Tower>();
        tower.SetTowerInfo(towerInfo);
        if (decorationMesh != null) Destroy(decorationMesh);    // Destroy the flora on the tile.

        return towerObj.GetComponent<Tower>();
    }


    /** Replace existing tower upon upgrade. */
    public Tower ReplaceTower(TowerInfo towerInfo) {
        Destroy(towerObj);
        towerBuildPosition = tileMesh.transform.position + towerOffset;
        towerObj = Instantiate(towerInfo.towerPrefab, towerBuildPosition, Quaternion.identity);
        tower = towerObj.GetComponent<Tower>();
        tower.SetTowerInfo(towerInfo);

        return towerObj.GetComponent<Tower>();
    }

    // NOTE; Should change to use the same raycasting procedure as per MapInput.cs
    private void OnMouseOver() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        if ((transform.position - playerObject.transform.position).magnitude > playerMovement.GetBuildDistance()) {
            tileRenderer.material.color = tooFarColor;
        } else {
            tileRenderer.material.color = hoverColor;
        }

        if (HasTower()) {
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
        
        if (HasTower()) {
            towerObj.gameObject.GetComponent<Outline>().enabled = false;
        } else if (decorationMesh != null) {
            decorationMesh.gameObject.GetComponent<Outline>().enabled = false;
        }
    }

}
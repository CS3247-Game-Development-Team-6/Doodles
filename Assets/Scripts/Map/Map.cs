using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    [Range(0.5f, 3f)]
    public float cellSize = 1.0f;
    public GameObject mapBase;
    public GameObject tileCopy;
    public GameObject fogCopy;
    // Number of cells in x by y grid.
    public Vector2Int gridSize = new Vector2Int(10, 6);
    private Cell[,] cells;

    private HashSet<GameObject> GetPathMarkers(Vector3 pos, int r, int c) {
        Collider[] colliders = Physics.OverlapSphere(pos, cellSize / 2);
        HashSet<GameObject> pathMarkerObjects = new HashSet<GameObject>();
        foreach (Collider coll in colliders) {
            if (coll.gameObject != mapBase) {
                Debug.Log(coll.gameObject);
                pathMarkerObjects.Add(coll.gameObject);
            }
        }
        return pathMarkerObjects;

    }

    private void Start() {
        Vector3 position = transform.position + (Vector3.one * 0.5f) * cellSize;
        cells = new Cell[gridSize.x, gridSize.y];
        HashSet<GameObject> pathMarkers = new HashSet<GameObject>();
        for (int r = 0; r < gridSize.x; r++) {
            for (int c = 0; c < gridSize.y; c++) {
                Vector3 cellPos = position + Vector3.right * c * cellSize;
                HashSet<GameObject> newPathMarkers = GetPathMarkers(cellPos, r, c);
                CellType cellType = CellType.NONE;
                if (newPathMarkers.Count > 0 ) {
                    cellType = CellType.WALK;
                    pathMarkers.UnionWith(newPathMarkers);
                }

                // Do something to identify the orientation of the fog
                bool isFog = false;
                Vector3 basePos = Vector3.forward;  // stub value indicating north
                
                // Set isFog to appropriate value
                if (basePos == Vector3.forward && r >= 5)
                {
                    isFog = true;
                }

                cells[r, c] = new Cell(new Vector2Int(r, c), cellPos, cellType, isFog);
                if (cellType != CellType.WALK) {                                // May need to change in the future if more enums are added
                    GameObject tile = Object.Instantiate(tileCopy, transform);
                    tile.name = string.Format("Cube {0}, {1}", r, c);
                    tile.transform.SetParent(mapBase.transform);
                    tile.transform.position = cells[r, c].Position;
                    tile.transform.localScale *= cellSize;
                }

                // Generate fog
                if (isFog == true)
                {
                    GameObject fog = Object.Instantiate(fogCopy, transform);
                    fog.name = string.Format("Fog {0}, {1}", r, c);
                    fog.transform.SetParent(mapBase.transform);
                    fog.transform.position = cells[r, c].Position;
                    fog.transform.localScale *= cellSize;
                }
            }
            position += Vector3.forward * cellSize;
        }
        tileCopy.SetActive(false);
        foreach (GameObject m in pathMarkers) {
            Destroy(m);
        }
        // Destroy the initial Fog on bottom left
        Destroy(GameObject.Find("Fog"));
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Vector3 position = transform.position;
        Vector3 width = Vector3.right * cellSize * gridSize.x;
        Vector3 height = Vector3.forward * cellSize * gridSize.y;
        for (int r = 0; r <= gridSize.x; r++) {
            Gizmos.DrawLine(position, position + height);
            position += Vector3.right * cellSize;
        }
        position = transform.position;
        for (int c = 0; c <= gridSize.y; c++) {
            Gizmos.DrawLine(position, position + width);
            position += Vector3.forward * cellSize;
        }
    }

}


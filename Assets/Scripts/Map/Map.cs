using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    [Range(0.5f, 3f)]
    public float cellSize = 1.0f;
    public GameObject mapBase;
    public GameObject tileCopy;
    public GameObject fogCopy;
    public int fogRows = 5;
    // Number of cells in x by y grid.
    public Vector2Int gridSize = new Vector2Int(10, 10);
    private Cell[,] cells;

    private void Awake() {
        Vector3 position = transform.position + (Vector3.one * 0.5f) * cellSize;
        cells = new Cell[gridSize.x, gridSize.y];
        for (int r = 0; r < gridSize.x; r++) {
            for (int c = 0; c < gridSize.y; c++) {
                Vector3 cellPos = position + Vector3.right * c * cellSize;
                Vector3 basePos = Vector3.forward;  // stub value indicating north

                // Do something to identify the orientation of the fog
                // bool isFog = basePos == Vector3.forward && r >= 5;
                bool isFog = false;
                cells[r, c] = new Cell(new Vector2Int(r, c), cellPos, CellType.NONE, isFog);
            }
            position += Vector3.forward * cellSize;
        }
    }

    private void Start() {
        for (int r = 0; r < gridSize.x; r++) {
            for (int c = 0; c < gridSize.y; c++) {
                Cell cell = cells[r, c];

                // Instantiate the default tile for every cell
                GameObject tile = Object.Instantiate(tileCopy, transform);
                tile.name = string.Format("Cube {0}, {1}", r, c);
                tile.transform.SetParent(mapBase.transform);
                tile.transform.position = cells[r, c].Position;
                tile.transform.localScale *= cellSize;
                cell.tile = tile;

                // Instantiate the fog tile for every cell
                GameObject fog = Object.Instantiate(fogCopy, transform);
                fog.name = string.Format("Fog {0}, {1}", r, c);
                fog.transform.SetParent(mapBase.transform);
                fog.transform.position = cells[r, c].Position;
                fog.transform.localScale *= cellSize;
                cell.fog = fog;

            }
        }
        tileCopy.SetActive(false);
        // Destroy the initial Fog on bottom left
        Destroy(GameObject.Find("Fog"));
    }
    private void Update() {
        for (int r = 0; r < gridSize.x; r++) {
            for (int c = 0; c < gridSize.y; c++) {
                Cell cell = cells[r, c];
                cell.tile.SetActive(cell.type == CellType.NONE);
                cell.fog.SetActive(cell.isFog);
            }
        }
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

    public Cell GetCellFromWorldPosition(Vector3 position) {
        Vector3 offset = position - transform.position;
        int x = (int)(offset.x / cellSize);
        int y = (int)(offset.z / cellSize);
        if (x > gridSize.x || y > gridSize.y || x < 0 || y < 0) 
            return cells[0, 0];
        return cells[x, y];
    }

    public void SetWaypoints(Cell[] waypointCells) {
        int n = waypointCells.Length;
        Debug.Log("set wayp");
        for (int i = 0; i < n - 1; i++) {
            Debug.Log(waypointCells[i].Index);
            waypointCells[i].type = CellType.WALK;
        }
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    [Range(0.5f, 3f)]
    public float cellSize = 1.0f;
    public GameObject tileCopy;
    // Number of cells in x by y grid.
    public Vector2Int gridSize = new Vector2Int(10, 6);
    private Cell[,] cells;

    private void Start() {
        Vector3 position = transform.position + (Vector3.one * 0.5f) * cellSize;
        cells = new Cell[gridSize.x, gridSize.y];
        for (int r = 0; r < gridSize.x; r++) {
            for (int c = 0; c < gridSize.y; c++) {
                cells[r, c] = new Cell(new Vector2Int(c, r), position + Vector3.right * c * cellSize);
                GameObject tile = Object.Instantiate(tileCopy, transform);
                tile.transform.position = cells[r, c].Position;
                tile.transform.localScale *= cellSize;
            }
            position += Vector3.forward * cellSize;
        }
        tileCopy.SetActive(false);
    }
    
    // TODO: Add waypoint editor to create paths easily

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


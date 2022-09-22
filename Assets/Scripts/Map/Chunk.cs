using UnityEngine;

public class Chunk : MonoBehaviour {
    [SerializeField] public float cellSize;
    [SerializeField] public Vector2Int gridSize;
    public int chunkId;
    public Vector2Int startPos;
    public Vector2Int spawnPos;
    public DIR spawnDir;
    public Chunk prevChunk;
    public Cell[,] cells;
    public Color highlightColor;

    public Vector2Int[,] dirGrid;
    public bool cellsGenerated { get; private set; }
    public bool prefabsGenerated;

    public void Init() {
        var f = Random.Range(0f, 1f) / 0.2 % 1;
        spawnDir = f < 0.5 ? DIR.RIGHT : DIR.UP;
        // x axis: rows; y axis: columns
        // RIGHT = (1,0), UP = (0, 1)
        if (prevChunk != null) { 
            startPos = prevChunk.spawnDir == DIR.RIGHT ? 
                new Vector2Int(0, prevChunk.spawnPos.y) : new Vector2Int(prevChunk.spawnPos.x, 0);
            spawnPos = spawnDir == DIR.RIGHT ? 
                new Vector2Int(gridSize.x-1, Mathf.FloorToInt(Random.Range(0, gridSize.y-1))) : 
                new Vector2Int(Mathf.FloorToInt(Random.Range(0, gridSize.x-1)), gridSize.y-1);
        } else if (spawnDir == DIR.RIGHT) {
            startPos = new Vector2Int(0, Mathf.FloorToInt(Random.Range(0, gridSize.y-1)));
            spawnPos = new Vector2Int(gridSize.x-1, Mathf.FloorToInt(Random.Range(0, gridSize.y-1)));
        } else if (spawnDir == DIR.UP) {
            startPos = new Vector2Int(Mathf.FloorToInt(Random.Range(0,gridSize.x-1)), 0);
            spawnPos = new Vector2Int(Mathf.FloorToInt(Random.Range(0, gridSize.x-1)), gridSize.y-1);
        }

        if (prevChunk != null) {
            transform.position = prevChunk.transform.position + 
                cellSize * (prevChunk.spawnDir == DIR.UP ? Vector3.right * gridSize.y : Vector3.forward * gridSize.x);
        }

        Vector3 placeInPosition = transform.position + new Vector3(0.5f, 0, 0.5f) * cellSize;
        cells = new Cell[gridSize.x, gridSize.y];
        
        for (int r = 0; r < gridSize.x; r++) {
            for (int c = 0; c < gridSize.y; c++) {
                Vector3 cellPos = placeInPosition + Vector3.right * c * cellSize;
                var typeOfCell = CellType.NONE;  // default every cell as a NONE-cell
                var yRotation = 0;
                var pathOrder = -1;
                Vector3 newRotation = new Vector3(0, yRotation, 0);
                cells[r, c] = new Cell(cellPos, typeOfCell, true, newRotation, pathOrder);
                // NOTE: per default we initialise all tiles as foggy, then clear some close to the base.
            }
            placeInPosition += Vector3.forward * cellSize;
        }
    }

    public bool GenerateBackupPath() {
        if (cellsGenerated) return true;
        Vector2Int currentPos = spawnPos;
        Vector2Int currentDir = -spawnDir.Vec();
        dirGrid = new Vector2Int[gridSize.x, gridSize.y];
        if (spawnDir == DIR.RIGHT) {
            int breakp = Random.Range(0, gridSize.x);
            while (currentPos.x != breakp) {
                dirGrid[currentPos.x, currentPos.y] = currentDir;
                currentPos += currentDir;
            }
            currentDir = currentPos.y < startPos.y ? DIR.DOWN.Vec() : DIR.UP.Vec();
            while (currentPos.y != startPos.y) {
                dirGrid[currentPos.x, currentPos.y] = currentDir;
                currentPos += currentDir;
            }
            currentDir = -spawnDir.Vec();
            while (currentPos.x != startPos.x) {
                dirGrid[currentPos.x, currentPos.y] = currentDir;
                currentPos += currentDir;
            }
            cellsGenerated = true;
            SetCellsOnPath();
            return true;
        } else {
            int breakp = Random.Range(0, gridSize.y);
            while (currentPos.y != breakp) {
                dirGrid[currentPos.x, currentPos.y] = currentDir;
                currentPos += currentDir;
            }
            currentDir = currentPos.x < startPos.x ? DIR.LEFT.Vec() : DIR.RIGHT.Vec();
            while (currentPos.x != startPos.x) {
                dirGrid[currentPos.x, currentPos.y] = currentDir;
                currentPos += currentDir;
            }
            currentDir = -spawnDir.Vec();
            while (currentPos.y != startPos.y) {
                dirGrid[currentPos.x, currentPos.y] = currentDir;
                currentPos += currentDir;
            }
            cellsGenerated = true;
            SetCellsOnPath();
            return true;
        }
    }

    public bool GenerateRandomPath(int tries, int minScore) {
        if (cellsGenerated) return true;
        if (tries == 0) {
            cellsGenerated = GenerateBackupPath();
            return true;
        }
        dirGrid = new Vector2Int[gridSize.x, gridSize.y];
        int length = 0;
        int score = 0;
        Vector2Int currentPos = spawnPos;
        Vector2Int currentDir = -spawnDir.Vec();
        int maxLength = gridSize.x * gridSize.y * 4;
        for (int _ = 0; _ < maxLength; _++) {
            Vector2Int nextPos = currentPos + currentDir;
            bool validCell = ContainsCell(nextPos);
            if (validCell && dirGrid[nextPos.x, nextPos.y] == Vector2Int.zero) {
                dirGrid[currentPos.x, currentPos.y] = currentDir;
                score += ScorePath(currentPos);
                if (nextPos == startPos) {
                    if (score >= minScore) {
                        cellsGenerated = true;
                        SetCellsOnPath();
                        return true;
                    }
                    else break;
                }
                length++;
                currentPos = nextPos;
            }
            currentDir = ((DIR)Random.Range(0, 4)).Vec();
        }

        return GenerateRandomPath(tries-1, minScore);
    }

    private void SetCellsOnPath() {
        Vector2Int lastPos = -Vector2Int.one;
        Vector2Int currentPos = spawnPos;
        int len = 0;
        while (lastPos != startPos) {
           Cell cell = cells[currentPos.x, currentPos.y];
            if (!ContainsCell(lastPos)) cell.type = CellType.SPAWN;
            else if (currentPos == startPos) cell.type = CellType.BASE;
            else if (dirGrid[lastPos.x, lastPos.y] != dirGrid[currentPos.x, currentPos.y]) {
                cell.type = CellType.CURVEPATH;
                // cell.rotation = new Vector3(0, GetRotationDeg(dirGrid[lastPos.y, lastPos.x], dirGrid[currentPos.y, currentPos.x]));
            } else {
                cell.type = CellType.STRAIGHTPATH;
                //cell.rotation = new Vector3(0, GetRotationDeg(dirGrid[lastPos.y, lastPos.x], dirGrid[currentPos.y, currentPos.x]));
            }
            cell.pathOrder = len++;
            lastPos = currentPos;
            currentPos += dirGrid[currentPos.x, currentPos.y];
            Debug.Log($"Cell {cell.type}");
        }
    }

    private int GetRotationDeg(Vector2Int first, Vector2Int second) {
        Vector2Int r = -Vector2Int.left;
        Vector2Int d = -Vector2Int.up;
        Vector2Int l = Vector2Int.left;
        Vector2Int u = Vector2Int.up;
        if (first == second) {
            if (first == r) return 0;
            else if (first == d) return 90;
            else if (first == l) return 180;
            else if (first == u) return 270;
        }

        if (first == l && second == u || first == d && second == r) return 0;
        else if (first == u && second == r || first == l && second == d) return 90;
        else if (first == r && second == d || first == u && second == l) return 180;
        else if (first == r && second == u || first == d && second == l) return 270;

        return 0;
    }

    private int ScorePath(Vector2Int cellCoord) {
        int score = 0;
        for (int c = -1; c <= 1; c++) {
            for (int r = -1; r <= 1; r++) {
                if (ContainsCell(cellCoord)) continue;
                if (dirGrid[cellCoord.y + c, cellCoord.x + r] == Vector2Int.zero) score++;
            }
        }
        return score;
    }

    public bool ContainsCell(Vector2Int cellCoord) {
        return 0 <= cellCoord.x && 0 <= cellCoord.y &&
            cellCoord.x < gridSize.x && cellCoord.y < gridSize.y;
    }


    public bool ContainsPosition(Vector3 pos) {
        return transform.position.x <= pos.x && transform.position.z <= pos.z &&
            pos.x < transform.position.x + cellSize * gridSize.x && 
            pos.z < transform.position.z + cellSize * gridSize.y;
    }

    private void OnDrawGizmos() {
        Gizmos.color = highlightColor;
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
        Vector3 offset = transform.position + 0.5f * cellSize * new Vector3(1, 0, 1);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(spawnPos.x, 0, spawnPos.y) + offset, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(startPos.x, 0, startPos.y) + offset, 0.1f);
    }

}

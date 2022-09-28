using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Waypoints), typeof(ChunkSpawner))]
public class Chunk : MonoBehaviour {
    public const int MIN_SIZE = 30;
    [SerializeField] public float cellSize;
    [SerializeField] public Vector2Int gridSize;
    public int chunkId;
    public Vector2Int startPos;
    public Vector2Int spawnPos;
    public DIR spawnDir;
    public Chunk prevChunk;
    public Chunk nextChunk;
    public Cell[,] cells;

    public Vector2Int[,] dirGrid;
    public bool cellsGenerated { get; private set; }
    public bool prefabsGenerated;
    public ChunkInfoScriptableObject levelInfo { get; private set; }
    public List<Cell> waypoints { get; private set; }
    public bool isVisible { get; private set; }
    private Barrier[] barriers;
    private ChunkSpawner chunkSpawner;
    public Barrier MainBarrier => barriers != null && barriers.Length == 4 ? barriers[(int)spawnDir] : null;

    public void Init(Transform barrierPrefab, ChunkInfoScriptableObject levelInfo) {
        this.levelInfo = levelInfo; 
        var f = Random.Range(0f, 1f) / 0.2 % 1;
        spawnDir = f < 0.5 ? DIR.RIGHT : DIR.UP;

        int minDiff = (gridSize.x + gridSize.y) / 4;
        int newX = 0, newY = 0, newX1 = 0, newY1 = 0;
        do {
            newX = Random.Range(0, gridSize.x - 1);
            newX1 = Random.Range(0, gridSize.x - 1);
            newY = Random.Range(0, gridSize.y - 1);
            newY1 = Random.Range(0, gridSize.y - 1);
        } while (Mathf.Abs(newX - newY) < minDiff);
        // x axis: rows; y axis: columns
        // RIGHT = (1,0), UP = (0, 1)
        if (prevChunk != null) { 
            startPos = prevChunk.spawnDir == DIR.RIGHT ? 
                new Vector2Int(0, prevChunk.spawnPos.y) : new Vector2Int(prevChunk.spawnPos.x, 0);
            spawnPos = spawnDir == DIR.RIGHT ? 
                new Vector2Int(gridSize.x-1, newY) : 
                new Vector2Int(newX, gridSize.y-1);
        } else if (spawnDir == DIR.RIGHT) {
            startPos = new Vector2Int(0, newY);
            spawnPos = new Vector2Int(gridSize.x-1, newY1);
        } else if (spawnDir == DIR.UP) {
            startPos = new Vector2Int(newX, 0);
            spawnPos = new Vector2Int(newX1, gridSize.y-1);
        }

        if (prevChunk != null) {
            transform.position = prevChunk.transform.position + 
                cellSize * (prevChunk.spawnDir == DIR.UP ? Vector3.right * gridSize.y : Vector3.forward * gridSize.x);
        }

        Vector3 placeInPosition = transform.position + new Vector3(0.5f, 0, 0.5f) * cellSize;
        cells = new Cell[gridSize.x, gridSize.y];

        int err = (int)Mathf.Pow(Mathf.Min(gridSize.x, gridSize.y), 2f)/5;
        for (int r = 0; r < gridSize.x; r++) {
            for (int c = 0; c < gridSize.y; c++) {
                Vector3 cellPos = placeInPosition + Vector3.right * c * cellSize;
                Vector2Int index = new Vector2Int(r, c);
                var typeOfCell = CellType.NONE;  // default every cell as a NONE-cell
                var yRotation = 0;
                var pathOrder = -1;
                Vector3 newRotation = new Vector3(0, yRotation, 0);
                bool isFog = levelInfo.isFogChunk && (startPos - index).sqrMagnitude - err > 0;
                cells[r, c] = new Cell(index, cellPos, typeOfCell, isFog, newRotation, pathOrder);
                // NOTE: per default we initialise all tiles as foggy, then clear some close to the base.
            }
            placeInPosition += Vector3.forward * cellSize;
        }

        barriers = new Barrier[4];
        for (int i = 0; i < 4; i++) {
            DIR d = (DIR)i;
            if (chunkId != 0 && d.Vec() == -prevChunk.spawnDir.Vec()) continue;

            Transform wall = Instantiate(barrierPrefab, transform);
            wall.name = $"Barrier {chunkId} {d}";
            barriers[i] = wall.GetComponent<Barrier>();
            barriers[i].Init(d);
            float wallWidth = 0f;
            float x = d == DIR.UP ? gridSize.y : d == DIR.DOWN ? wallWidth/2 : ((float)gridSize.y / 2);
            float z = d == DIR.RIGHT ? gridSize.x : d == DIR.LEFT ? wallWidth/2 : ((float)gridSize.x / 2);
            wall.position = transform.position + new Vector3(x, 1, z);
            x = d == DIR.UP || d == DIR.DOWN ? wallWidth : gridSize.y;
            z = d == DIR.RIGHT || d == DIR.LEFT ? wallWidth : gridSize.x;
            wall.localScale = new Vector3(x, 2, z);
        }

        chunkSpawner = GetComponent<ChunkSpawner>();
        //  First disable chunkSpawner.
        chunkSpawner.enabled = false;
        chunkSpawner.Init(levelInfo, cells[spawnPos.x, spawnPos.y].position);
    }

    public void OpenBarrier() {
        // set trigger to proceed to next level
        if (MainBarrier == null) {
            Debug.Log("No barrier to open.");
            return;
        }

        MainBarrier.GetComponent<Collider>().isTrigger = true;
        Debug.Log($"Opening the barrier for {nextChunk}: {MainBarrier}");
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
            currentDir = currentPos.y < startPos.y ? DIR.UP.Vec() : DIR.DOWN.Vec();
            while (currentPos.y != startPos.y) {
                dirGrid[currentPos.x, currentPos.y] = currentDir;
                currentPos += currentDir;
            }
            currentDir = currentPos.x < startPos.x ? DIR.RIGHT.Vec() : DIR.LEFT.Vec();
            while (currentPos.x != startPos.x) {
                dirGrid[currentPos.x, currentPos.y] = currentDir;
                currentPos += currentDir;
            }
        } else {
            int breakp = Random.Range(0, gridSize.y);
            while (currentPos.y != breakp) {
                dirGrid[currentPos.x, currentPos.y] = currentDir;
                currentPos += currentDir;
            }
            currentDir = currentPos.x < startPos.x ? DIR.RIGHT.Vec() : DIR.LEFT.Vec();
            while (currentPos.x != startPos.x) {
                dirGrid[currentPos.x, currentPos.y] = currentDir;
                currentPos += currentDir;
            }
            currentDir = currentPos.y < startPos.y ? DIR.UP.Vec() : DIR.DOWN.Vec();
            while (currentPos.y != startPos.y) {
                dirGrid[currentPos.x, currentPos.y] = currentDir;
                currentPos += currentDir;
            }
        }
        cellsGenerated = true;
        SetCellsOnPath();
        return true;
    }

    public bool GenerateRandomPath(int tries, int minScore) {
        if (cellsGenerated) return false;
        if (tries == 0 || gridSize.x * gridSize.y < MIN_SIZE) {
            cellsGenerated = GenerateBackupPath();
            return true;
        }
        dirGrid = new Vector2Int[gridSize.x, gridSize.y];
        int length = 0;
        int score = 0;
        int minLength = (gridSize.x + gridSize.y) / 2;
        int maxLength = (gridSize.x + gridSize.y) * 3 / 2;
        Vector2Int currentPos = spawnPos;
        Vector2Int currentDir = -spawnDir.Vec();
        int maxTries = gridSize.x * gridSize.y * 4;

        // Adjust based on Manhattan Distance
        minScore -= Mathf.Abs(spawnPos.x - startPos.x) + Mathf.Abs(spawnPos.y - startPos.y);


        for (int _ = 0; _ < maxTries; _++) {
            Vector2Int nextPos = currentPos + currentDir;
            bool validCell = ContainsCell(nextPos);
            if (validCell && dirGrid[nextPos.x, nextPos.y] == Vector2Int.zero) {
                dirGrid[currentPos.x, currentPos.y] = currentDir;
                score += ScorePath(currentPos);
                if (nextPos == startPos) {
                    if (score >= minScore && minLength < length && length < maxLength) {
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
        waypoints = new List<Cell>();
        while (lastPos != startPos) {
           Cell cell = cells[currentPos.x, currentPos.y];
            if (!ContainsCell(lastPos)) {
                cell.type = CellType.SPAWN;
                waypoints.Add(cell);
            } else if (currentPos == startPos) {
                cell.type = CellType.BASE;
                waypoints.Add(cell);
            } else if (dirGrid[lastPos.x, lastPos.y] != dirGrid[currentPos.x, currentPos.y]) {
                cell.type = CellType.CURVEPATH;
                cell.rotation = new Vector3(0, GetRotationDeg(dirGrid[lastPos.x, lastPos.y], dirGrid[currentPos.x, currentPos.y]), 0);
                waypoints.Add(cell);
            } else {
                cell.type = CellType.STRAIGHTPATH;
                cell.rotation = new Vector3(0, GetRotationDeg(dirGrid[lastPos.x, lastPos.y], dirGrid[currentPos.x, currentPos.y]), 0);
            }
            cell.pathOrder = len++;
            lastPos = currentPos;
            currentPos += dirGrid[currentPos.x, currentPos.y];
        }
    }

    public void SetWaypoints() {
        if (!prefabsGenerated) {
            Debug.LogError("Prefabs not generated yet, waypoints cannot be set.");
            return;
        }
        GetComponent<Waypoints>().SetWaypoints(waypoints);

    }

    public void StartSpawning() {
        chunkSpawner.enabled = true;
    }

    private int GetRotationDeg(Vector2Int first, Vector2Int second) {
        Vector2Int u = DIR.UP.Vec();
        Vector2Int r = DIR.RIGHT.Vec();
        Vector2Int d = DIR.DOWN.Vec();
        Vector2Int l = DIR.LEFT.Vec();
        if (first == second) {
            if (first == l) return 0;
            else if (first == u) return 90;
            else if (first == r) return 180;
            else if (first == d) return 270;
        }

        if (first == l && second == u || first == d && second == r) return 0;
        else if (first == r && second == u || first == d && second == l) return 90;
        else if (first == r && second == d || first == u && second == l) return 180;
        else if (first == u && second == r || first == l && second == d) return 270;

        return 0;
    }

    private int ScorePath(Vector2Int cellCoord) {
        int score = 0;
        for (int r = -1; r <= 1; r++) {
            for (int c = -1; c <= 1; c++) {
                if (!ContainsCell(cellCoord + new Vector2Int(r,c))) continue;
                if (dirGrid[cellCoord.x + r, cellCoord.y + c] == Vector2Int.zero) score++;
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
            pos.x < transform.position.x + cellSize * gridSize.y && 
            pos.z < transform.position.z + cellSize * gridSize.x;
    }

    public void SetVisible(bool state) {
        isVisible = state;
        for (int r = 0; r < gridSize.x; r++) {
            for (int c = 0; c < gridSize.y; c++) {
                if (cells[r,c].fog != null) {
                    bool isHidden = cells[r, c].isFog || !state;
                    cells[r, c].fog.SetActive(isHidden);
                    cells[r, c].tile.SetActive(!isHidden);
                }
            }
        }
    }

    private void OnDrawGizmos() {
        Vector3 position = transform.position;
        Vector3 width = Vector3.right * cellSize * gridSize.y;
        Vector3 height = Vector3.forward * cellSize * gridSize.x;
        for (int c = 0; c <= gridSize.y; c++) {
            Gizmos.DrawLine(position, position + height);
            position += Vector3.right * cellSize;
        }
        position = transform.position;
        for (int r = 0; r <= gridSize.x; r++) {
            Gizmos.DrawLine(position, position + width);
            position += Vector3.forward * cellSize;
        }
        Vector3 offset = transform.position + 0.5f * cellSize * new Vector3(1, 0, 1);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(spawnPos.y, 0, spawnPos.x) + offset, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(startPos.y, 0, startPos.x) + offset, 0.1f);
    }

}

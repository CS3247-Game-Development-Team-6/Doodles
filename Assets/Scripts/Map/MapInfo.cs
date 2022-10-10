using UnityEngine;

[CreateAssetMenu(fileName = "New MapInfo", menuName = "Map")]
public class MapInfo : ScriptableObject {

    [Header("Prefabs")]

    [SerializeField] public GameObject tilePrefab;
    [SerializeField] public GameObject fogPrefab;
    [SerializeField] public GameObject basePrefab;
    [SerializeField] public GameObject spawnPrefab;
    [SerializeField] public GameObject curvePrefab;
    [SerializeField] public GameObject straightPrefab;
    [SerializeField] public GameObject waypointPrefab;

    [Header("LevelInfo")]
    
    [SerializeField] public string levelName;
    [SerializeField] public Vector2Int gridSize;
    [SerializeField] public int minScore = 20;
    [SerializeField,Range(0,1)] public float startingInkFraction;
    [SerializeField,Range(0,1)] public float inkRegenRate;
    [SerializeField] public ChunkInfoScriptableObject[] chunkInfo;

    public void GeneratePrefabs(Chunk chunk) {
        if (!chunk.cellsGenerated) return;
        if (chunk.prefabsGenerated) return;

        Vector3 offset = new Vector3(1, 0, 1) * chunk.cellSize;
        for (int r = 0; r < chunk.cells.GetLength(0); r++) {
            for (int c = 0; c < chunk.cells.GetLength(1); c++) {
                Cell cell = chunk.cells[r, c];
                GameObject tileToPlace = tilePrefab;
                GameObject tile;
                if (cell.type == CellType.CURVEPATH) {
                    tileToPlace = curvePrefab;
                } else if (cell.type == CellType.STRAIGHTPATH) {
                    tileToPlace = straightPrefab;
                } else if (cell.type == CellType.SPAWN) {
                    tileToPlace = spawnPrefab;
                } else if (cell.type == CellType.BASE) {
                    tileToPlace = basePrefab;
                }
                tile = Instantiate(tileToPlace, cell.position, Quaternion.Euler(cell.rotation));
                tile.name = $"{tileToPlace} {r}, {c}";
                tile.transform.SetParent(chunk.transform);
                tile.transform.localScale *= chunk.cellSize;
                if (tileToPlace.GetComponent<Node>() != null) {
                    tile.GetComponent<Node>().cell = cell;
                }

                GameObject fog = Instantiate(fogPrefab, chunk.transform);
                fog.name = $"Fog {r}, {c}";
                fog.transform.localScale *= chunk.cellSize;
                fog.transform.position = tile.transform.position;
                fog.GetComponent<Fog>().cell = cell;

                chunk.cells[r,c].tile = tile;
                chunk.cells[r,c].fog = fog;
                fog.SetActive(cell.isFog);
                tile.SetActive(!cell.isFog);
            }
        }
        chunk.prefabsGenerated = true;
    }

    public GameObject GenerateWaypoint(int index, Transform parent) {
        GameObject waypoint = Instantiate(waypointPrefab, parent, true);
        waypoint.name = $"Waypoint {index}";
        return waypoint;
    }
    
}

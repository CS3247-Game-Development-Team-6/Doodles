using System.Collections.Generic;
using UnityEngine;

public static class DirExtensions {
    public static Vector2Int Vec(this DIR dir) {
        switch (dir) {
            // x axis: rows; y axis: columns
            case DIR.LEFT: return new Vector2Int(-1, 0);
            case DIR.RIGHT: return new Vector2Int(1, 0);
            case DIR.UP: return new Vector2Int(0, 1);
            case DIR.DOWN: return new Vector2Int(0, -1);
            default: return Vector2Int.zero;
        }
    }
    public static Vector3 Rot(this DIR dir) {
        switch (dir) {
            case DIR.RIGHT: return new Vector3(0,0,0);
            case DIR.DOWN: return new Vector3(0,90,0);
            case DIR.LEFT: return new Vector3(0,180,0);
            case DIR.UP: return new Vector3(0,270,0);
            default: return Vector3.zero;
        }
    }
}

public enum DIR { 
    LEFT, RIGHT, UP, DOWN
}

public class Map : MonoBehaviour {
    private const int ORIG_SCALE = 10;
    private const int GENERATION_MAX_TRIES = 10;
    // Pairs of chunks and next dir (Down/Right)
    [SerializeField] private Vector2Int chunkSize;
    [SerializeField] private MapInfo mapInfo;
    [SerializeField] private int numVisibleChunks;
    [SerializeField] private bool isEndless;

    [SerializeField] private Transform basePlane;
    [SerializeField] private Barrier barrierPrefab;
    [SerializeField] private GameObject waypointEmpty;
    [SerializeField] private ParticleSystem poofEffect;

    private Chunk currentChunk;
    private List<Chunk> chunkList = new List<Chunk>();
    private Transform player;
    private WaveSpawner waveSpawner;
    private int lastChunkId = -1;

    private void Start() {
        player = FindObjectOfType<PlayerMovement>().transform;
        waveSpawner = FindObjectOfType<WaveSpawner>();
        // choose one edge for start and the other edge for end
        for (int chunk = 0; chunk < numVisibleChunks; chunk++) {
            GameObject newChunk = new GameObject("Chunk" + chunkList.Count);
            newChunk.transform.parent = transform;
            newChunk.transform.localPosition = Vector3.zero;
            newChunk.AddComponent<Chunk>();
            currentChunk = newChunk.GetComponent<Chunk>();
            currentChunk.chunkId = chunk;
            currentChunk.cellSize = 1f;
            currentChunk.gridSize = chunkSize;
            currentChunk.highlightColor = Color.cyan;
            if (chunk > 0) currentChunk.prevChunk = chunkList[chunk - 1];
            if (barrierPrefab != null) currentChunk.Init(barrierPrefab.transform);
            chunkList.Add(currentChunk);
        }
        if (numVisibleChunks > 0) currentChunk = chunkList[0];
    }

    private void DeactivateChunk(Chunk chunk) {
        chunk.SetActive(false);
    }

    private void ActivateChunk() {
        if (currentChunk.prefabsGenerated) currentChunk.SetActive(true);
        else if (currentChunk.GenerateRandomPath(GENERATION_MAX_TRIES, 3)) {
            mapInfo.GeneratePrefabs(currentChunk);
            SetWaypoints(currentChunk.waypoints);
        }
        basePlane.localScale = new Vector3(currentChunk.gridSize.y, 1, currentChunk.gridSize.x) / ORIG_SCALE;
        basePlane.position = currentChunk.transform.position + new Vector3(currentChunk.gridSize.y, -0.1f, currentChunk.gridSize.x) / 2;
        waveSpawner.spawnPoint = currentChunk.cells[currentChunk.spawnPos.x, currentChunk.spawnPos.y].tile.transform;
    }
    private Vector3[] SetWaypoints(List<Cell> waypointCells) {
        // sort array based on their order in the path
        // Array.Sort(waypointCells, (oneCell, otherCell) => oneCell.pathOrder.CompareTo(otherCell.pathOrder));
        Vector3[] waypointsPosition = new Vector3[waypointCells.Count];
        GameObject prevWaypoint = null;
        for (int i = 0; i < waypointCells.Count; i++) {
            waypointsPosition[i] = waypointCells[i].position;
            GameObject waypoint = mapInfo.GenerateWaypoint(i, waypointEmpty.transform);
            if (waypoint != null) {
                waypoint.transform.position = i == waypointCells.Count - 1
                    ? changeEnemyTargetWaypoint(waypointCells[i].position, prevWaypoint)
                    : waypointCells[i].position;
            }
            prevWaypoint = waypoint;
        }
        
        return waypointsPosition;
    }

    private Vector3 changeEnemyTargetWaypoint(Vector3 basePosition, GameObject closestWaypoint) {
        if (closestWaypoint == null) return basePosition;

        var delta = basePosition - closestWaypoint.transform.position;

        delta = delta.normalized;
        delta *= 0.5f;  // this moves the waypoint 0.5 units towards the closest waypoint. can be in-/decreased.

        return basePosition - delta;
    }


    private void Update() {
        currentChunk.highlightColor = Color.cyan;
        Chunk newChunk = currentChunk;
        int tries = 0;
        for (; tries < chunkList.Count && !newChunk.ContainsPosition(player.position); tries++) {
            if (currentChunk.chunkId == newChunk.chunkId + 1) break;
            newChunk = chunkList[(currentChunk.chunkId + 1) % chunkList.Count];
        }
        if (tries < chunkList.Count) {
            currentChunk = newChunk;
            foreach (Chunk c in chunkList) {
                // if (Mathf.Abs(c.chunkId - currentChunk.chunkId) > 1) DeactivateChunk(c);
                DeactivateChunk(c);
            }
            currentChunk.highlightColor = Color.green;
            ActivateChunk();
        }
    }
}

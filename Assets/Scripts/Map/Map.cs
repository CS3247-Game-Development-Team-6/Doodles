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
    private int numChunks;
    // [SerializeField] private bool isEndless;

    [SerializeField] private Transform basePlane;
    [SerializeField] private Barrier barrierPrefab;
    [SerializeField] private GameObject waypointEmpty;
    [SerializeField] private ParticleSystem poofEffect;

    public Chunk currentChunk { get; private set; }
    private List<Chunk> chunkList = new List<Chunk>();
    private Transform player;
    //private WaveSpawner waveSpawner;

    private void Start() {
        player = FindObjectOfType<PlayerMovement>().transform;
        // waveSpawner = FindObjectOfType<WaveSpawner>();
        numChunks = mapInfo.levelInfo.Length;
        // choose one edge for start and the other edge for end
        for (int chunk = 0; chunk < numChunks; chunk++) {
            GameObject newChunk = new GameObject("Chunk" + chunkList.Count);
            newChunk.transform.parent = transform;
            newChunk.transform.localPosition = Vector3.zero;
            newChunk.AddComponent<Chunk>();
            currentChunk = newChunk.GetComponent<Chunk>();
            currentChunk.chunkId = chunk;
            currentChunk.cellSize = 1f;
            currentChunk.gridSize = chunkSize;
            if (chunk > 0) {
                currentChunk.prevChunk = chunkList[chunk - 1];
                currentChunk.prevChunk.nextChunk = currentChunk;
            }
            if (barrierPrefab != null) currentChunk.Init(barrierPrefab.transform, mapInfo.levelInfo[chunk]);
            chunkList.Add(currentChunk);
        }
        if (numChunks > 0) currentChunk = chunkList[0];
        var startCell = currentChunk.startPos;
        var playerCell = startCell;
        while (!currentChunk.ContainsCell(playerCell) || playerCell == startCell) {
            playerCell = startCell + new Vector2Int(Random.Range(0, 3) - 1, Random.Range(0, 3) - 1);
        }
        player.position = currentChunk.cells[playerCell.x, playerCell.y].position + player.up * 0.25f + Vector3.up * 0.5f;

        // Reset in order to generate paths in next Update()
        currentChunk = null;
    }

    private void DeactivateChunk(Chunk chunk) {
        if (chunk.isCurrentChunk) {
            chunk.SetVisible(false);
            currentChunk.GetComponent<Waypoints>().DeactivateLocalWaypoints();
        }
    }

    private void ActivateChunk(Chunk chunk, bool isFocus) {
        Debug.Log($"Activating {chunk}:  visibility {isFocus}");
        if (chunk.prefabsGenerated) chunk.SetVisible(isFocus);
        else if (chunk.GenerateRandomPath(GENERATION_MAX_TRIES, 3)) {
            // is false if cells already generated
            mapInfo.GeneratePrefabs(chunk);
            chunk.SetWaypoints();
            chunk.SetVisible(isFocus);
            /*
            if (isFocus) {
                chunk.GetComponent<Waypoints>().SetWaypoints(chunk.waypoints, mapInfo.waypointPrefab);
                chunk.GetComponent<Waypoints>().ActivateLocalWaypoints();
                waveSpawner.SetNewLevel(chunk.levelInfo);
            }
            */
        }

        if (!isFocus) return;

        basePlane.localScale = new Vector3(chunk.gridSize.y, 1, chunk.gridSize.x) / ORIG_SCALE;
        basePlane.position = chunk.transform.position + new Vector3(chunk.gridSize.y, -0.1f, chunk.gridSize.x) / 2;
        chunk.OpenNext();
        /*
        if (waveSpawner.AreWavesCleared()) {
            if (chunk.chunkId == chunkList.Count - 1) waveSpawner.WinGame();
            else {
                chunk.OpenNext();
            }
        }
        */
    }

    private void Update() {
        // change behaviour to: if current chunk does not contain position,
        // then look for the one that does and adjust accordingly
        if (currentChunk != null && currentChunk.ContainsPosition(player.position)) return;
        if (chunkList.Count == 0) return;

        Chunk newChunk = currentChunk == null ? chunkList[0] : currentChunk;
        int tries = 0;
        for (; tries < chunkList.Count && !newChunk.ContainsPosition(player.position); tries++) {
            if (currentChunk.chunkId == newChunk.chunkId + 1) break;
            newChunk = chunkList[(currentChunk.chunkId + 1) % chunkList.Count];
        }
        if (tries < chunkList.Count) {
            currentChunk = newChunk;
            foreach (Chunk c in chunkList) {
                if (c.prevChunk == newChunk) 
                    ActivateChunk(c, newChunk.isCaptured);
                else if (c != newChunk) 
                    DeactivateChunk(c);
            }
            ActivateChunk(currentChunk, true);
        }
    }
}

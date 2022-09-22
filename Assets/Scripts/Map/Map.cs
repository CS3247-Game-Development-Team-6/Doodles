using System.Collections.Generic;
using UnityEngine;

public static class DirExtensions {
    public static Vector2Int Vec(this DIR dir) {
        switch (dir) {
            // x axis: rows; y axis: columns
            case DIR.LEFT: return new Vector2Int(-1, 0);
            case DIR.RIGHT: return new Vector2Int(1, 0);
            case DIR.UP: return new Vector2Int(0, -1);
            case DIR.DOWN: return new Vector2Int(0, 1);
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
    private const int GENERATION_MAX_TRIES = 10;
    // Pairs of chunks and next dir (Down/Right)
    [SerializeField] private Vector2Int chunkSize;
    [SerializeField] private MapInfo mapInfo;
    [SerializeField] private int numVisibleChunks;
    [SerializeField] private bool isEndless;

    private Chunk currentChunk;
    private List<Chunk> chunkList = new List<Chunk>();
    private Transform player;

    private void Start() {
        player = FindObjectOfType<PlayerMovement>().transform;
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
            currentChunk.Init();
            chunkList.Add(currentChunk);
        }
        if (numVisibleChunks > 0) currentChunk = chunkList[0];
        // mapInfo.GeneratePrefabs(currentChunk);
    }

    private void ActivateChunk() {
        if (currentChunk.GenerateRandomPath(10,10)) {
        // if (currentChunk.GenerateBackupPath()) {
            if (!currentChunk.prefabsGenerated) mapInfo.GeneratePrefabs(currentChunk);
        }
    }

    private void Update() {
        currentChunk.highlightColor = Color.cyan;
        int firstChunk = currentChunk.chunkId;
        int tries = 0;
        for (; tries < 100 && !currentChunk.ContainsPosition(player.position); tries++) {
            if (firstChunk == currentChunk.chunkId + 1) break;
            currentChunk = chunkList[(currentChunk.chunkId + 1) % chunkList.Count];
        }
        currentChunk.highlightColor = Color.green;
        ActivateChunk();
    }
}

using System;
using System.IO;
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
    private const int GENERATION_MAX_TRIES = 20;
    // Pairs of chunks and next dir (Down/Right)
    private Vector2Int chunkSize;
    [SerializeField] private MapInfo mapInfo;
    public MapInfo MapInfo => mapInfo;
    private int numChunks;
    // [SerializeField] private bool isEndless;

    [SerializeField] private Transform basePlane;
    [SerializeField] private Barrier barrierPrefab;
    [SerializeField] private GameObject waypointEmpty;
    [SerializeField] private ParticleSystem poofEffect;

    public Chunk currentChunk { get; private set; }
    private List<Chunk> chunkList = new List<Chunk>();
    private Transform player;
    private WaveUI waveUI;
    private OnScreenTutorialUI tutorialUI;
    private int chunkWavesCleared;
    public int WavesCleared => chunkWavesCleared + (currentChunk == null ? 0 : currentChunk.GetComponent<ChunkSpawner>().WavesStarted - 1);

    private void Start() {
        string path = Application.dataPath + "/map.json";
        File.Delete(path);
        /*
        */
        player = FindObjectOfType<PlayerMovement>().transform;
        waveUI = FindObjectOfType<WaveUI>();
        tutorialUI = FindObjectOfType<OnScreenTutorialUI>();
        numChunks = mapInfo.chunkInfo.Length;
        chunkSize = mapInfo.gridSize;
        chunkWavesCleared = 0;
        if (!Load()) {
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
                    chunkList[chunk - 1].nextChunk = currentChunk;
                }
                if (barrierPrefab != null) currentChunk.Init(barrierPrefab.transform, mapInfo.chunkInfo[chunk]);
                chunkList.Add(currentChunk);
            }
        }
        if (chunkList.Count == 0) return;

        // Initial Chunk Activation
        ActivateCurrentChunk();
        var startCell = currentChunk.startPos;
        var playerCell = startCell;
        while (!currentChunk.ContainsCell(playerCell) || playerCell == startCell) {
            playerCell = startCell + new Vector2Int(UnityEngine.Random.Range(0, 3) - 1, UnityEngine.Random.Range(0, 3) - 1);
        }
        player.position = currentChunk.cells[playerCell.x, playerCell.y].position + player.up * 0.25f + Vector3.up * 0.5f;
        tutorialUI.SetNotes(currentChunk.chunkInfo);
    }

    public void Save() {
        ChunkData[] chunkDatas = new ChunkData[currentChunk.chunkId+1];
        for (int i = 0; i <= currentChunk.chunkId; i++) {
            chunkDatas[i] = chunkList[i].GetChunkData();
        }
        MapData data = new MapData {
            mapInfo = this.mapInfo,
            chunkDatas = chunkDatas,
            chunkWavesCleared = chunkWavesCleared,
        };

        string json = JsonUtility.ToJson(data);
        string path = Application.dataPath + "/map.json";
        File.WriteAllText(path, json);
        Debug.Log($"Saving: {json} at {path}");
    }

    public bool Load() {
        string path = Application.dataPath + "/map.json";
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            MapData data = JsonUtility.FromJson<MapData>(json);
            Debug.Log($"Chunk datas {data.chunkDatas}");
            foreach (ChunkData chunk in data.chunkDatas) {
                GameObject newChunk = new GameObject("Chunk" + chunkList.Count);
                newChunk.transform.parent = transform;
                newChunk.AddComponent<Chunk>();
                currentChunk = newChunk.GetComponent<Chunk>();
                currentChunk.LoadChunkData(barrierPrefab.transform, chunk);
                currentChunk.gridSize = chunkSize;
                if (chunk.chunkId > 0) {
                    currentChunk.prevChunk = chunkList[chunk.chunkId - 1];
                    chunkList[chunk.chunkId - 1].nextChunk = currentChunk;
                }
                if (barrierPrefab != null) currentChunk.Init(barrierPrefab.transform, mapInfo.chunkInfo[chunk.chunkId]);
                chunkList.Add(currentChunk);
            }
            return true;
        } else {
            Debug.Log("no save found.");
            return false;
        }
    }

    private void DeactivatePrevChunk(object sender, EventArgs e) {
        DeactivateChunk(currentChunk.prevChunk);
    }

    private void DeactivateChunk(Chunk chunk) {
        if (chunk.IsVisible) {
            chunk.SetVisible(false);
            currentChunk.GetComponent<Waypoints>().DeactivateLocalWaypoints();
        }
    }

    private bool GenerateChunk(Chunk chunk) {
        // Debug.Log($"Generating {chunk}");
        if (chunk.prefabsGenerated) return true;
        if (chunk.GenerateRandomPath(GENERATION_MAX_TRIES, mapInfo.minScore)) {
            mapInfo.GeneratePrefabs(chunk);
            chunk.SetVisible(false);
            return true;
        }
        return false;
    }

    private void SetCurrentChunk(Chunk chunk) {
        Debug.Log($"Activating {chunk}");
        if (!GenerateChunk(chunk)) return;

        // Chunk data and objects are generated
        basePlane.localScale = new Vector3(chunk.gridSize.y, 1, chunk.gridSize.x) / ORIG_SCALE;
        basePlane.position = chunk.transform.position + new Vector3(chunk.gridSize.y, -0.1f, chunk.gridSize.x) / 2;
        ChunkSpawner chunkSpawner = chunk.GetComponent<ChunkSpawner>();
        chunk.SetWaypoints();
        chunk.StartSpawning();
        chunkSpawner.OnWaveEnd += OpenNextChunk;
        chunkSpawner.OnWaveEnd += tutorialUI.SetNotesForNextChunk;
        waveUI.SetSpawner(chunkSpawner);
        chunk.SetVisible(true);
        currentChunk = chunk;
    }

    private void OpenNextChunk(object sender, EventArgs e) {
        if (currentChunk.nextChunk == null) { 
            ChunkSpawner.WinGame();
            return;
        }
        ChunkSpawner chunkSpawner = currentChunk.GetComponent<ChunkSpawner>();
        chunkWavesCleared += chunkSpawner.WavesStarted;
        currentChunk.nextChunk.SetVisible(true);
        currentChunk.OpenBarrier();
        tutorialUI.Reset();
        currentChunk.MainBarrier.CrossBarrier += ActivateNextChunk;
        // currentChunk.MainBarrier.CloseBarrier += DeactivatePrevChunk;
    }

    private void ActivateNextChunk(object sender, EventArgs e) {
        if (!(sender is Barrier)) return;
        Chunk nextChunk = ((Barrier)sender).NextChunk;
        SetCurrentChunk(nextChunk);
        if (nextChunk.nextChunk != null) {
            GenerateChunk(nextChunk.nextChunk);
            nextChunk.nextChunk.SetVisible(false);
        }
    }

    private void ActivateCurrentChunk() {
        // Map will look for the currentChunk that Player is standing on
        if (currentChunk != null && currentChunk.ContainsPosition(player.position)) return;
        Chunk newChunk = currentChunk == null ? chunkList[0] : currentChunk;
        int tries = 0;
        for (; tries < chunkList.Count && !newChunk.ContainsPosition(player.position); tries++) {
            if (currentChunk.chunkId == newChunk.chunkId + 1) break;
            newChunk = chunkList[(currentChunk.chunkId + 1) % chunkList.Count];
        }
        if (tries < chunkList.Count) {
            currentChunk = newChunk;
            foreach (Chunk c in chunkList) {
                if (c.prevChunk == newChunk) {
                    GenerateChunk(c);
                    c.SetVisible(false);
                } else if (c != newChunk) {
                    DeactivateChunk(c);
                }
            }
            SetCurrentChunk(currentChunk);
        }
    }

    private class MapData {
        public MapInfo mapInfo;
        public ChunkData[] chunkDatas;
        public int chunkWavesCleared;
    }
}

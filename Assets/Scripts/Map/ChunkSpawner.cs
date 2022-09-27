using System;
using UnityEngine;

[RequireComponent(typeof(Chunk))]
public class ChunkSpawner : MonoBehaviour {
    public static int totalWaveCount { get; private set; }

    public int numEnemiesAlive;
    public int numEnemiesLeftInWave;
    public bool isSpawningEnemy;

    public WaveSet[] waves;
    public int waveIndex { get; private set; }

    public Vector3 spawnPointPos { get; set; }
    public float timeBetweenWaves { get; private set; }  = 2f;

    public float countdownTimer { get; private set; }  = 2f; // decrease with time, countdown for new wave
    private bool initialized;

    // Event to publish to UI elements. WaveUI elements should subscribe.
    public event EventHandler OnWaveActivity;
    public event EventHandler OnWaveEnd;

    public int WavesLeft => waves == null ? 0 : Mathf.Max(0, waves.Length - waveIndex);

    public void Init(ChunkInfoScriptableObject levelInfo, Vector3 spawnPointPos) {
        initialized = true;
        numEnemiesAlive = 0;
        numEnemiesLeftInWave = 0;
        isSpawningEnemy = false;
        countdownTimer = timeBetweenWaves;
        waveIndex = 0;

        if (levelInfo != null) waves = levelInfo.waves;
        this.spawnPointPos = spawnPointPos;
    }

    private void OnEnable() {
    }

    public void ResetTimer() {
        countdownTimer = 0f;
    }

    private void Update() {
        if (!initialized) { return; }

        // Pause if player is viewing tutorial
        if (!PlayerPrefs.HasKey(OnScreenTutorialUI.OnScreenTutorialPref)
            || PlayerPrefs.GetInt(OnScreenTutorialUI.OnScreenTutorialPref) != 1) {
            return;
        }

        OnWaveActivity?.Invoke(this, EventArgs.Empty);

        if (numEnemiesAlive > 0 || isSpawningEnemy) {
            return;
        }

        if (countdownTimer <= 0f) {
            if (waveIndex < waves.Length) {
                SpawnWave();
                waveIndex = Mathf.Min(waves.Length, waveIndex+1);
                isSpawningEnemy = true;
                countdownTimer = waveIndex == waves.Length ? 0 : timeBetweenWaves;
            } else {
                OnWaveEnd?.Invoke(this, EventArgs.Empty);
                Debug.Log("Ended waves");
                this.enabled = false;
            }
            return;
        }

        countdownTimer -= Time.deltaTime;
        countdownTimer = Mathf.Clamp(countdownTimer, 0f, Mathf.Infinity);
    }

    private void SpawnWave() {
        if (waveIndex == waves.Length) return;

        totalWaveCount++;

        WaveSet waveToSpawn = waves[waveIndex];

        numEnemiesLeftInWave = waveToSpawn.getTotalEnemy();
        StartCoroutine(waveToSpawn.StartWave(this));
    }

    public static void WinGame() {
        FindObjectOfType<GameStateManager>().WinGame();
    }

    public void SpawnEnemy(GameObject _enemy) {
        GameObject enemy = Instantiate(_enemy, spawnPointPos, Quaternion.identity);
        enemy.GetComponent<Enemy>().Init(this);
        numEnemiesAlive++;
    }

}

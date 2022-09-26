using UnityEngine;

[RequireComponent(typeof(Chunk))]
public class ChunkSpawner : MonoBehaviour {
    public static int totalWaveCount { get; private set; }

    public int numEnemiesAlive;
    public int numEnemiesLeftInWave;
    public bool isSpawningEnemy;

    [SerializeField] private LevelInfoScriptableObject levelInfo;
    public WaveSet[] waves;
    public int waveIndex { get; private set; }

    public Transform spawnPoint { get; set; }
    public float timeBetweenWaves = 5f;

    private float countdownTimer = 5f; // decrease with time, countdown for new wave
    private WaveUI waveUI;
    private bool initialized;

    public void Init(LevelInfoScriptableObject levelInfo) {
        initialized = true;
        numEnemiesAlive = 0;
        numEnemiesLeftInWave = 0;
        isSpawningEnemy = false;
        countdownTimer = timeBetweenWaves;
        waveIndex = 0;
        waveUI = FindObjectOfType<WaveUI>();

        this.levelInfo = levelInfo;

        if (levelInfo != null) waves = levelInfo.waves;
    }

    private void ResetTimer() {
        countdownTimer = 0f;
    }

    private void Update() {
        if (!initialized) {
            Debug.Log($"Chunk Spawner of {name}");
            return;
        }

        waveUI.UpdateTimerIndicator(countdownTimer, timeBetweenWaves);
        waveUI.UpdateWaves(waveIndex);
        waveUI.UpdateEnemies(numEnemiesLeftInWave);
        
        // Pause if player is viewing tutorial
        if (!PlayerPrefs.HasKey(OnScreenTutorialUI.OnScreenTutorialPref)
            || PlayerPrefs.GetInt(OnScreenTutorialUI.OnScreenTutorialPref) != 1) {
            return;
        }

        if (numEnemiesAlive > 0 || isSpawningEnemy) {
            return;
        }

        if (countdownTimer <= 0f) {
            waveIndex = Mathf.Min(waves.Length, waveIndex+1);
            if (waveIndex < waves.Length) {
                SpawnWave();
                isSpawningEnemy = true;
                countdownTimer = waveIndex == waves.Length - 1 ? 0: timeBetweenWaves;
            } else {
                GetComponent<Waypoints>().enabled = false;
            }
            return;
        }

        // isSkipWaveCountdownButtonVisible = true;

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
        Instantiate(_enemy, spawnPoint.position, spawnPoint.rotation);
        numEnemiesAlive++;
    }

    public void SetNumEnemiesForTheWave(int num) {
        numEnemiesLeftInWave = num;
    }

}

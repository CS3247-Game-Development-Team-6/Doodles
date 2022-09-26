using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {
    [SerializeField] private Text wavesCounterUI;

    public static int numEnemiesAlive; // keep track of how many enemies alive then only spawn new wave
    // public static int wavesCounter;
    public static int numEnemiesLeftInWave;
    public static bool isSpawningEnemy;
    public static int totalWaveCount { get; private set; }

    public WaveSet[] waves;
    public Transform spawnPoint { get; set; }
    public float timeBetweenWaves = 5f;
    public IndicatorUI waveCountdownIndicator;
    public Text enemiesLeftText;
    public LevelInfoScriptableObject levelInfo;
    public Button skipWaveCountdownButton;

    private bool isSkipWaveCountdownButtonVisible;
    private float countdownTimer = 3f; // decrease with time, countdown for new wave
    public int waveIndex { get; private set; }


    void Start() {
        //SetNewLevel(levelInfo);
    }

    public bool AreWavesCleared() {
        return waves.Length == waveIndex && !isSpawningEnemy;
    }

    public void SetNewLevel(LevelInfoScriptableObject levelInfo) {
        Debug.Log("Resetting wave");
        numEnemiesAlive = 0;
        numEnemiesLeftInWave = 0;
        isSpawningEnemy = false;
        countdownTimer = timeBetweenWaves;

        isSkipWaveCountdownButtonVisible = true;
        skipWaveCountdownButton.onClick.AddListener(ButtonOnClick);

        this.levelInfo = levelInfo;
        if (levelInfo != null) waves = levelInfo.waves;

        waveIndex = -1;
    }

    void ButtonOnClick() {
        isSkipWaveCountdownButtonVisible = false;

        countdownTimer = 0f; // reset timer 
        UpdateTimerIndicator(countdownTimer);
    }

    void Update() {
        /*
        skipWaveCountdownButton.gameObject.SetActive(isSkipWaveCountdownButtonVisible);
        enemiesLeftText.text = string.Format("{0}", numEnemiesLeftInWave);
        wavesCounterUI.text = "Wave " + string.Format("{0}", waveIndex+1);

        // Pause if player is viewing tutorial
        if (!PlayerPrefs.HasKey(OnScreenTutorialUI.OnScreenTutorialPref)
            || PlayerPrefs.GetInt(OnScreenTutorialUI.OnScreenTutorialPref) != 1) {
            isSkipWaveCountdownButtonVisible = false;
            return;
        }

        if (numEnemiesAlive > 0 || isSpawningEnemy) {
            isSkipWaveCountdownButtonVisible = false;
            return;
        }

        if (countdownTimer <= 0f) {
            waveIndex = Mathf.Min(waves.Length, waveIndex+1);
            if (waveIndex < waves.Length) {
                SpawnWave();
                isSpawningEnemy = true;
                countdownTimer = waveIndex == waves.Length - 1 ? 0: timeBetweenWaves;
            }
            return;
        }
        isSkipWaveCountdownButtonVisible = true;

        countdownTimer -= Time.deltaTime;
        countdownTimer = Mathf.Clamp(countdownTimer, 0f, Mathf.Infinity);

        UpdateTimerIndicator(countdownTimer);
        */

    }

    public void WinGame() {
        GetComponent<GameStateManager>().WinGame();
        this.enabled = false;
    }

    void UpdateTimerIndicator(float time) {
        waveCountdownIndicator.rawValue = (int)(time * 100);
        waveCountdownIndicator.maxValue = (int)(timeBetweenWaves * 100);
    }

    void SpawnWave() {
        if (waveIndex == waves.Length) return;

        totalWaveCount++;

        WaveSet waveToSpawn = waves[waveIndex];

        numEnemiesLeftInWave = waveToSpawn.getTotalEnemy();
        StartCoroutine(waveToSpawn.StartWave(this));
    }

    public void SpawnEnemy(GameObject _enemy) {
        Instantiate(_enemy, spawnPoint.position, spawnPoint.rotation);
        numEnemiesAlive++;
    }

    public void SetNumEnemiesForTheWave(int num) {
        numEnemiesLeftInWave = num;
    }

}

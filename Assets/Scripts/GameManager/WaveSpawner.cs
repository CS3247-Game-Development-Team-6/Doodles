using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {
    [SerializeField] private Text wavesCounterUI;

    public static int numEnemiesAlive; // keep track of how many enemies alive then only spawn new wave
    public static int wavesCounter;
    public static int numEnemiesLeftInWave;
    public static bool isSpawningEnemy;

    public WaveSet[] waves;
    public Transform spawnPoint { get; set; }
    public float timeBetweenWaves = 5f;
    public IndicatorUI waveCountdownIndicator;
    public Text enemiesLeftText;
    public LevelInfoScriptableObject levelInfo;
    public Button skipWaveCountdownButton;

    private bool isSkipWaveCountdownButtonVisible;
    private float countdownTimer = 3f; // decrease with time, countdown for new wave
    private int waveIndex = 0;

    public bool waveCleared { get; private set; }

    void Start() {
        SetNewLevel(levelInfo);
    }

    public void SetNewLevel(LevelInfoScriptableObject levelInfo) {
        Debug.Log("Resetting wave");
        waveCleared = false;
        numEnemiesAlive = 0;
        numEnemiesLeftInWave = 0;
        isSpawningEnemy = false;
        countdownTimer = timeBetweenWaves;
        wavesCounter = 0;

        isSkipWaveCountdownButtonVisible = true;
        skipWaveCountdownButton.onClick.AddListener(ButtonOnClick);

        this.levelInfo = levelInfo;
        if (levelInfo != null) waves = levelInfo.waves;

        waveIndex = 0;
    }

    void ButtonOnClick() {
        isSkipWaveCountdownButtonVisible = false;

        countdownTimer = 0f; // reset timer 
        UpdateTimerIndicator(countdownTimer);
    }

    void Update() {
        skipWaveCountdownButton.gameObject.SetActive(isSkipWaveCountdownButtonVisible);
        enemiesLeftText.text = string.Format("{0}", numEnemiesLeftInWave);
        wavesCounterUI.text = "Wave " + string.Format("{0}", wavesCounter);

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

        if (waveIndex == waves.Length) {
            waveCleared = true;
        }

        if (countdownTimer <= 0f) {
            isSpawningEnemy = waveIndex < waves.Length;
            SpawnWave();
            countdownTimer =  waveIndex < waves.Length ? timeBetweenWaves : 0;
            return;
        }
        isSkipWaveCountdownButtonVisible = true;

        countdownTimer -= Time.deltaTime;
        countdownTimer = Mathf.Clamp(countdownTimer, 0f, Mathf.Infinity);

        UpdateTimerIndicator(countdownTimer);

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
        wavesCounter++;

        WaveSet waveToSpawn = waves[waveIndex];

        numEnemiesLeftInWave = waveToSpawn.getTotalEnemy();
        StartCoroutine(waveToSpawn.StartWave(this));

        waveIndex++;
    }

    public void SpawnEnemy(GameObject _enemy) {
        Instantiate(_enemy, spawnPoint.position, spawnPoint.rotation);
        numEnemiesAlive++;
    }

    public void SetNumEnemiesForTheWave(int num) {
        numEnemiesLeftInWave = num;
    }

}

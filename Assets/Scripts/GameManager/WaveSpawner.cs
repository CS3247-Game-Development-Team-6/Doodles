using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    // keep track of how many enemies alive then only spawn new wave

    public static int numEnemiesAlive;

    // enemies left counter
    public static int numEnemiesLeftInWave;
    public static bool isSpawningEnemy;

    public Wave[] waves;

    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;

    public IndicatorUI waveCountdownIndicator;
    public Text enemiesLeftText;

    public LevelInfoScriptableObject levelInfo;

    public Button skipWaveCountdownButton;
    // boolean to set the button visible
    private bool isSkipWaveCountdownButtonVisible;

    // decrease with time, countdown for new wave
    private float countdownTimer = 3f;

    private int waveIndex = 0;

    private OnScreenTutorialUI tutorialScript;

    void Start()
    {
        numEnemiesAlive = 0;
        numEnemiesLeftInWave = 0;
        isSpawningEnemy = false;
        countdownTimer = timeBetweenWaves;

        isSkipWaveCountdownButtonVisible = true;
        skipWaveCountdownButton.onClick.AddListener(buttonOnClick);

        tutorialScript = GameObject.Find("Canvas").GetComponentInChildren<OnScreenTutorialUI>();

        if (levelInfo != null) waves = levelInfo.waves;
    }

    void buttonOnClick()
    {
        isSkipWaveCountdownButtonVisible = false;

        // reset timer 
        countdownTimer = 0f;
        UpdateTimerIndicator(countdownTimer);
    }

    // Update is called once per frame
    void Update()
    {
        skipWaveCountdownButton.gameObject.SetActive(isSkipWaveCountdownButtonVisible);
        enemiesLeftText.text = string.Format("{0}", numEnemiesLeftInWave);

        // stops when first time seeing tutorial
        if (!PlayerPrefs.HasKey(OnScreenTutorialUI.OnScreenTutorialPref) 
            || PlayerPrefs.GetInt(OnScreenTutorialUI.OnScreenTutorialPref) != 1)
        {
            isSkipWaveCountdownButtonVisible = false;
            return;
        }

        if (numEnemiesAlive > 0 || isSpawningEnemy)
        {
            isSkipWaveCountdownButtonVisible = false;
            return;
        }

        // win the game
        if (waveIndex == waves.Length)
        {
            GetComponent<GameManager>().WinGame();

            // disable this script
            this.enabled = false;
        }

        if (countdownTimer <= 0f)
        {
            isSpawningEnemy = true;
            SpawnWave();
            countdownTimer = timeBetweenWaves;         
            return;
        }
        isSkipWaveCountdownButtonVisible = true;

        countdownTimer -= Time.deltaTime;
        countdownTimer = Mathf.Clamp(countdownTimer, 0f, Mathf.Infinity);

        UpdateTimerIndicator(countdownTimer);
        
    }

    void UpdateTimerIndicator(float time)
    {
        waveCountdownIndicator.rawValue = (int)(time * 100);
        waveCountdownIndicator.maxValue = (int)(timeBetweenWaves * 100);
    }

    // can pause the func execution
    void SpawnWave()
    {
        //keep track of how many rounds survive
        GameManager.rounds++;

        Wave waveToSpawn = waves[waveIndex];

        numEnemiesLeftInWave = waveToSpawn.getTotalEnemy();
        StartCoroutine(waveToSpawn.StartWave(this));

        waveIndex++;
    }

    public void SpawnEnemy(GameObject _enemy)
    {
        Instantiate(_enemy, spawnPoint.position, spawnPoint.rotation);
        numEnemiesAlive++;
    }

    public void SetNumEnemiesForTheWave(int num)
    {
        numEnemiesLeftInWave = num;
    }

}

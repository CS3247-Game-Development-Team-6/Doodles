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
    public static bool isSpawningEnemy = false;

    public Wave[] waves;

    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;

    public IndicatorUI waveCountdownIndicator;
    public Text enemiesLeftText;

    // decrease with time, countdown for new wave
    private float countdownTimer = 3f;

    private int waveIndex = 0;

    void Start()
    {
        numEnemiesAlive = 0;
        numEnemiesLeftInWave = 0;
    }

    // Update is called once per frame
    void Update()
    {

        enemiesLeftText.text = ": " + string.Format("{0}", numEnemiesLeftInWave);

        if (numEnemiesAlive > 0 || isSpawningEnemy)
        {
            return;
        }

        // win the game
        if (waveIndex == waves.Length)
        {
            Debug.Log("LEVEL WON!");

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

        countdownTimer -= Time.deltaTime;

        countdownTimer = Mathf.Clamp(countdownTimer, 0f, Mathf.Infinity);
        waveCountdownIndicator.rawValue = (int)(countdownTimer * 100);
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

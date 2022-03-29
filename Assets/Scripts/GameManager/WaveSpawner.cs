using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    // keep track of how many enemies alive then only spawn new wave
    public static int numEnemiesAlive = 0;

    public Wave[] waves;

    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;

    public Text waveCountdownText;
    public IndicatorUI waveCountdownIndicator;

    // decrease with time, countdown for new wave
    private float countdownTimer = 3f;

    private int waveIndex = 0;

    // Update is called once per frame
    void Update()
    {
        if (numEnemiesAlive > 0)
        {
            return;
        }

        if (countdownTimer <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdownTimer = timeBetweenWaves;
            return;
        }

        countdownTimer -= Time.deltaTime;

        countdownTimer = Mathf.Clamp(countdownTimer, 0f, Mathf.Infinity);
        waveCountdownIndicator.rawValue = (int)(countdownTimer * 100);
        waveCountdownIndicator.maxValue = (int)(timeBetweenWaves * 100);

        // format 00.00s
        waveCountdownText.text = string.Format("{0:00.00}", countdownTimer);
    }

    // can pause the func execution
    IEnumerator SpawnWave()
    {
        //keep track of how many rounds survive
        GameManager.rounds++;

        Wave waveToSpawn = waves[waveIndex];

        for (int i = 0; i < waveToSpawn.count; i++)
        {
            SpawnEnemy(waveToSpawn.enemy);
            yield return new WaitForSeconds(1f / waveToSpawn.rate);
        }
        waveIndex++;

        if (waveIndex == waves.Length)
        {
            Debug.Log("LEVEL WON!");

            // disable this script
            this.enabled = false;
        }
    }

    void SpawnEnemy(GameObject _enemy)
    {
        Instantiate(_enemy, spawnPoint.position, spawnPoint.rotation);
        numEnemiesAlive++;
    }
}

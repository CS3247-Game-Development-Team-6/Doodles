using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;

    public Text waveCountdownText;

    // decrease with time
    private float countdownTimer = 2f;

    // waveNumber = number of enemy
    private int waveIndex = 0;

    // Update is called once per frame
    void Update()
    {
        if (countdownTimer <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdownTimer = timeBetweenWaves;
        }
        countdownTimer -= Time.deltaTime;

        waveCountdownText.text = Mathf.Round(countdownTimer).ToString();
    }

    // can pause the func execution
    IEnumerator SpawnWave()
    {
        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
        waveIndex++;
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}

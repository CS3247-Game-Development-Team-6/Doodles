using System;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour {
    private ChunkSpawner chunkSpawner;
    [SerializeField] private Text wavesCounterUI;
    [SerializeField] private IndicatorUI waveCountdownIndicator;
    [SerializeField] private Text enemiesLeftText;
    [SerializeField] private Button skipWaveCountdownButton;

    public void SetSpawner(ChunkSpawner chunkSpawner) {
        this.chunkSpawner = chunkSpawner;
        chunkSpawner.OnWaveActivity += UpdateAllDisplay;
    }

    public void UpdateAllDisplay(object sender, EventArgs e) {
        if (!(sender is ChunkSpawner)) {
            Debug.LogWarning($"{sender} is not a ChunkSpawner, is ignored.");
            return;
        } 
        
        if (chunkSpawner != (ChunkSpawner)sender) {
            chunkSpawner = (ChunkSpawner)sender;
            Debug.Log($"{sender} registers its skip function");
            skipWaveCountdownButton.onClick.AddListener(() => SetSkipActivity(false));
        }

        skipWaveCountdownButton.gameObject.SetActive(chunkSpawner.numEnemiesAlive == 0 && !chunkSpawner.isSpawningEnemy);

        UpdateEnemies(chunkSpawner.numEnemiesLeftInWave);
        UpdateWaves(chunkSpawner.waveIndex);
        UpdateTimerIndicator(chunkSpawner.countdownTimer, chunkSpawner.timeBetweenWaves);
    }

    private void SetSkipActivity(bool isActive) {
        chunkSpawner.ResetTimer();
        skipWaveCountdownButton.gameObject.SetActive(isActive);
    }

    private void UpdateTimerIndicator(float time, float timeBetweenWaves) {
        waveCountdownIndicator.rawValue = (int)(time * 100);
        waveCountdownIndicator.maxValue = (int)(timeBetweenWaves * 100);
    }

    private void UpdateEnemies(int numEnemiesLeft) {
        enemiesLeftText.text = numEnemiesLeft.ToString();
    }

    private void UpdateWaves(int waveIndex) {
        wavesCounterUI.text = $"Wave {waveIndex}";
    }
}

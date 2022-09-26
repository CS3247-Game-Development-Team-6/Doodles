using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour {
    [SerializeField] private Text wavesCounterUI;
    [SerializeField] private IndicatorUI waveCountdownIndicator;
    [SerializeField] private Text enemiesLeftText;
    [SerializeField] private Button skipWaveCountdownButton;
    [SerializeField] private bool isSkipVisible;
    public void UpdateTimerIndicator(float time, float timeBetweenWaves) {
        waveCountdownIndicator.rawValue = (int)(time * 100);
        waveCountdownIndicator.maxValue = (int)(timeBetweenWaves * 100);
    }

    public void UpdateEnemies(int numEnemiesLeft) {
        enemiesLeftText.text = numEnemiesLeft.ToString();
    }

    public void UpdateWaves(int waveIndex) {
        enemiesLeftText.text = $"Wave {waveIndex+1}";
    }
}

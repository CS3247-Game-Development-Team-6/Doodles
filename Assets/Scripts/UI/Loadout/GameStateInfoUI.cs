using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateInfoUI : MonoBehaviour
{
    public MapInfo mapInfo;
    public Image inkSlider;
    public TextMeshProUGUI inkText;
    public Image enemySlider;
    public TextMeshProUGUI enemyText;
    [SerializeField] private TMP_Text textCountDown;
    private InkManager inkManager;
    private ChunkSpawner chunkSpawner;
    private int totalChunks;
    private int totalWaves;

    public void SetInkManager(InkManager inkManager)
    {
        this.inkManager = inkManager;
    }

    public void SetChunkSpawner(ChunkSpawner chunkSpawner)
    {
        this.chunkSpawner = chunkSpawner;
    }

    private void Start()
    {
        mapInfo = Loadout.mapToLoad;
        if (!mapInfo)
        {
            Debug.Log("No map loaded.");
            return;
        }

        inkSlider.fillAmount = mapInfo.startingInkFraction;
        int startingInk = (int)(mapInfo.startingInkFraction * mapInfo.totalInk);
        inkText.text = $"{startingInk} / {mapInfo.totalInk}";
        enemySlider.fillAmount = 0f;
        totalWaves = 0;
        foreach (var chunk in mapInfo.chunkInfo)
        {
            totalWaves += chunk.waves.Length;
        }
        totalChunks = mapInfo.chunkInfo.Length;
        enemyText.text = $"{mapInfo.chunkInfo.Length} Levels\n{totalWaves} Total Waves";
        textCountDown.gameObject.SetActive(false);



    }

    private string GetWaveStatus()
    {
        int chunkNum = chunkSpawner.chunkIndex;
        int waveNum = chunkSpawner.WavesStarted;
        int totalWavesInChunk = chunkSpawner.waves.Length;
        return $"\nWave {waveNum} / {totalWavesInChunk}\nin level {chunkNum} / {totalChunks}";
    }

    private float GetChunkCompletion()
    {
        float waveNum = chunkSpawner.waveIndex;
        float totalWavesInChunk = chunkSpawner.waves.Length;
        return totalWavesInChunk == 0 ? 0 : waveNum / totalWavesInChunk;
    }

    private void Update()
    {
        if (inkManager != null)
        {
            inkSlider.fillAmount = inkManager.InkFraction;
            inkText.text = inkManager.InkString;
        }

        if (chunkSpawner != null)
        {
            if (Mathf.RoundToInt(chunkSpawner.countdownTimer) > 0 & Mathf.RoundToInt(chunkSpawner.countdownTimer) != Mathf.RoundToInt(chunkSpawner.timeBetweenWaves))
            {

                textCountDown.text = Mathf.RoundToInt(chunkSpawner.countdownTimer).ToString();
                textCountDown.gameObject.SetActive(true);




            }
            else if (Mathf.RoundToInt(chunkSpawner.countdownTimer) == 0 | chunkSpawner.countdownTimer == chunkSpawner.timeBetweenWaves)
            {
              
                textCountDown.gameObject.SetActive(false);


            }

            enemyText.text = GetWaveStatus();
            enemySlider.fillAmount = GetChunkCompletion();

        }
    }


}
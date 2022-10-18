using UnityEngine;

[CreateAssetMenu(fileName = "LevelNum_Map_Chunk_ChunkId", menuName = "Chunk Info")]
public class ChunkInfo : ScriptableObject {
    // These are to be removed eventually
    public string levelName;
    public float startingInkPercentage;
    // 
    public WaveSet[] waves;
    public bool isFogChunk;
    public Note[] notes;
}

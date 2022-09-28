using UnityEngine;

// MIGRATING: Eventually to become ChunkInfoScriptableObject, editable from MapInfo
[CreateAssetMenu(fileName = "NewLevel", menuName = "Level Info")]
public class ChunkInfoScriptableObject : ScriptableObject {
    // These are to be removed eventually
    public string levelName;
    public float startingInkPercentage;
    // 
    public WaveSet[] waves;
    public bool isFogChunk;
    public Note[] notes;
}
using UnityEngine;

// MIGRATING: Eventually to become ChunkInfoScriptableObject, editable from MapInfo
[CreateAssetMenu(fileName = "NewLevel", menuName = "Level Info")]
public class LevelInfoScriptableObject : ScriptableObject {
    // These are to be removed eventually
    public string levelName;
    public float startingInkPercentage;
    // 
    public WaveSet[] waves;
    public bool isFogChunk;
}

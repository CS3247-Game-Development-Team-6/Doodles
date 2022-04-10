using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Level Info")]
public class LevelInfoScriptableObject : ScriptableObject
{
    // These  are dynamically set at Start() in WaveSpawner and Player.
    public string levelName;
    public float startingInkPercentage;
    public Wave[] waves;
}

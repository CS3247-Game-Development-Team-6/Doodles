using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell Info")]
public class SpellInfo : ScriptableObject {
    public const int MAX_LEVELS = 4;
    public string name;
    [TextArea(3, 6)] public string description;

    [SerializeField, Range(0, 1000)] public int cost;
    [SerializeField, Range(0, MAX_LEVELS)] public int level;
    [SerializeField, Range(0, 100)] public float cooldownTime;
    [SerializeField, Range(0, 100)] public float effectTime;
    public Sprite sprite;
}

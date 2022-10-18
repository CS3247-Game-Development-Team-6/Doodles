using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Info", menuName = "Enemy Info")]
public class EnemyInfo : ScriptableObject {
    /**
     * UI stuff
     */
    public string enemyName;
    [TextArea(3, 6)] public string description;

    /**
     * Basic stats
     */
    public float speed;
    public float health;
    public int defense;
    public Sprite sprite;

    /**
     * Amount of ink after killing enemy
     */
    public float inkGained;
    public EnemyShootingInfo shotInfo;

    /**
     * Custom visual effect
     */
    public GameObject deathEffect;

    /**
     * Element type
     */
    [Header("Element stuff")]
    public ElementInfo element;
    public float damageMultiplier;

    /**
     * Invulnerability for a period of time
     */
    [Header("Invulnerable")]
    public bool isInvulnerable;
    public float duration;
    public float cooldown;

    /**
     * Spawnable after death
     */
    [Header("Spawnable")]
    public bool isSpawnable;
    public int spawnCount;
    public GameObject spawnPrefab;
    public GameObject spawnEffect;
    public bool dieOnBase;

}

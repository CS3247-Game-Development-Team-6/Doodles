using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Info", menuName = "Enemy Info")]
public class EnemyInfo : ScriptableObject {
    public float speed;
    public float health;
    public int defense;
    public float inkGained;
    public GameObject deathEffect;
    public ElementEffectType elementType;
}

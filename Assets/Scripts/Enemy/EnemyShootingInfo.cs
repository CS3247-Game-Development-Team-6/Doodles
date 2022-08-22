using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Shooting Info", menuName = "Enemy Shooting Info")]
public class EnemyShootingInfo : ScriptableObject {
    public float range;
    public float fireRate;
    public float bulletSpeed;
    public int bulletDamage;
}

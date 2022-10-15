using UnityEngine;

public class Beyblade : Tower {
    
    // List that holds all the enemies in range
    private Collider[] enemyInRange;
    private Collider[] enemyTrigger;
    private const bool PENETRATE_TARGET = false;
    private bool isExploded = false;
    public const string VFX_NAME = "VFX";
    private Transform rotationBase;
    private Transform firePoint;

    [Header("Effect Prefabs")]
    public GameObject selfDestructEffect;
    public GameObject selfDestructSound;

    public override void SetTowerInfo(TowerInfo towerInfo) {
        base.SetTowerInfo(towerInfo);
        rotationBase = transform.Find(Tower.ROTATION_BASE_NAME);
        firePoint = rotationBase.Find(Tower.FIRE_POINT_NAME);
    }

    public void Start() {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    public void UpdateTarget() {
        // A list of objects contained in a sphere of radius = towerInfo.range, centered at the tower's origin
        enemyInRange = Physics.OverlapSphere(transform.position, range);
        // A list of objects contained in a sphere of radius = 0.1f, centered at the tower's orb
        enemyTrigger = Physics.OverlapSphere(firePoint.position, 0.1f);
    }

    public override void Shoot() {
        base.Shoot();
        foreach (var enemy in enemyInRange) {
            if (enemy.gameObject.tag == ENEMY_TAG) {
                GameObject bulletObj = (GameObject) Instantiate (
                    bulletPrefab, 
                    enemy.transform.position, 
                    enemy.transform.rotation
                );

                Bullet bullet = bulletObj.GetComponent<Bullet>();
                bullet.SetBulletInfo(towerInfo);
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                bullet.SetBulletDamage(CalculateDamage(distance));
                bullet.Seek(enemy.transform, PENETRATE_TARGET);
            }
        }
        TriggerEffect();
        Destroy(gameObject, 0.5f);
    }

    // Trigger visual and sound effects
    private void TriggerEffect() {
        GameObject selfDestructEffectParticle = (GameObject)Instantiate(selfDestructEffect, transform.position, transform.rotation);
        selfDestructEffectParticle.transform.parent = transform;
        GameObject selfDestructEffectAudio = (GameObject)Instantiate(selfDestructSound, transform.position, transform.rotation);
        selfDestructEffectAudio.transform.SetParent(transform);
        selfDestructEffectAudio.GetComponent<AudioSource>().Play();
    }

    // Calculate damage based on distance of the enemy from the origin of the landmine orb
    private int CalculateDamage(float distance) {
        float cosDamage = .5f * Mathf.Cos((1/(0.4f*range) * distance)) + .5f; 
        return Mathf.FloorToInt(towerInfo.damage * cosDamage);
    }

    public override void Update() {
        // Checks if an enemy has entered the trigger and if the landmine has not exploded yet
        if (enemyTrigger != null && !isExploded) {
            foreach (var enemy in enemyTrigger) {
                if (enemy.gameObject.tag == ENEMY_TAG) {
                    Shoot();
                    isExploded = true;
                }
            }
        }
    }

    // Debug Tool: Draw a sphere to show the range of the tower
    // private void OnDrawGizmosSelected() {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, 3);
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(firePoint.position, 0.1f);
    // }
}

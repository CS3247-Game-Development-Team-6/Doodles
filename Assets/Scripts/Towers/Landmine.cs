using UnityEngine;

public class Landmine : Tower {
    
    // List that holds all the enemies in range
    private Collider[] enemyInRange;
    private const bool PENETRATE_TARGET = false;
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

    // Find all objects in range
    public void UpdateTarget() {
        enemyInRange = Physics.OverlapSphere(transform.position, range);
    }

    public override void Shoot()
    {
        base.Shoot();
        foreach (var enemy in enemyInRange)
        {
            if (enemy.gameObject.tag == ENEMY_TAG) {
                GameObject bulletObj = (GameObject) Instantiate (
                    bulletPrefab, 
                    enemy.transform.position, 
                    enemy.transform.rotation
                );

                Bullet bullet = bulletObj.GetComponent<Bullet>();
                bullet.SetBulletInfo(towerInfo);
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                bullet.SetBulletDamage(calculateDamage(distance));
                bullet.Seek(enemy.transform, PENETRATE_TARGET);
            }
        }
        TriggerEffect();
        Destroy(gameObject, 1f);
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
    private float calculateDamage(float distance) {
        float cosDamage = Mathf.Cos((1/(range/3) * distance)) + 1; 
        return towerInfo.damage * cosDamage;
    }

    public override void Update() {
        // Create a overlap sphere to detect enemy that touches the landmine orbs
        Collider[] enemyTrigger = Physics.OverlapSphere(firePoint.position, 0.1f);
        foreach (var enemy in enemyTrigger){
            if (enemy.gameObject.tag == ENEMY_TAG) {
                Shoot();
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

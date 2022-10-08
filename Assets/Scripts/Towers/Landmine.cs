using UnityEngine;

public class Landmine : Tower {
    private Collider[] enemyInRange;
    private const bool PENETRATE_TARGET = false;
    private Transform rotationBase;
    private Transform firePoint;

    public override void SetTowerInfo(TowerInfo towerInfo) {
        base.SetTowerInfo(towerInfo);
        rotationBase = transform.Find(Tower.ROTATION_BASE_NAME);
        firePoint = rotationBase.Find(Tower.FIRE_POINT_NAME);
    }

    public void Start() {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

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
        // TODO: Create tower self destruction effect
        // TODO: Create tower self destruction sound
        Destroy(gameObject);
    }

    private float calculateDamage(float distance) {
        float cosDamage = Mathf.Cos((1/(range/3) * distance)) + 1; 
        return towerInfo.damage * cosDamage;
    }

    // Update is called once per frame
    public override void Update() {
        Collider[] enemyTrigger = Physics.OverlapSphere(firePoint.position, 0.1f);
        foreach (var enemy in enemyTrigger)
        {
            if (enemy.gameObject.tag == ENEMY_TAG) {
                Shoot();
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(firePoint.position, 0.1f);
    }
}

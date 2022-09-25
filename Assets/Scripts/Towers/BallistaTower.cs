using UnityEngine;

public class BallistaTower : Tower {

    private float rotationSpeed = 5f;
    private Transform rotationBase;
    private Transform firePoint;
    private float fireCountdown = 0f;
    private Transform target;

    private const bool PENETRATE_TARGET = false;

    public override void SetTowerInfo(TowerInfo towerInfo) {
        base.SetTowerInfo(towerInfo);
        rotationBase = transform.Find(Tower.ROTATION_BASE_NAME);
        firePoint = rotationBase.Find(Tower.FIRE_POINT_NAME);
    }

    void Start() {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    /** Checks every second for the closest enemy in range. */
    void UpdateTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(ENEMY_TAG);

        float shortestDistance = Mathf.Infinity;
        
        GameObject nearestEnemy = null;
        target = null;
        
        foreach (GameObject enemy in enemies) {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance) {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= this.range) {
            target = nearestEnemy.transform;
        }
    }

    public override void Shoot() {
        base.Shoot();
        GameObject bulletObj = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); ;
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.SetBulletInfo(towerInfo);
        
        if (bullet != null) {
            bullet.Seek(target, PENETRATE_TARGET);
        }
    }

    public override void Update() {
        base.Update();

        if (health <= 0) return;
        if (!target) return;

        
        // Enemy target lock on 
        Vector3 dir = target.position - transform.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(rotationBase.rotation, lookAtRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        rotationBase.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (fireCountdown <= 0f) {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }
    
}

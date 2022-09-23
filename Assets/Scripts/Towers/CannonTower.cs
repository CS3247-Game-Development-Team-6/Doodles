using UnityEngine;
using UnityEngine.UI;

public class CannonTower : Tower {

    private float rotationSpeed = 5f;
    private Transform rotationBase;
    private Transform firePoint;
    private float fireCountdown = 0f;
    private Transform target;
    private Image healthBar;
    private float maxHealth;
    private const bool PENETRATE_TARGET = false;
    private GameObject smokeEffect;

    public override void SetTowerInfo(TowerInfo towerInfo) {
        base.SetTowerInfo(towerInfo);
        rotationBase = transform.Find(Tower.ROTATION_BASE_NAME);
        firePoint = rotationBase.Find(Tower.FIRE_POINT_NAME);
        healthBar = transform.Find("TowerHealthCanvas/HealthBG/HealthBar").GetComponent<Image>();
        maxHealth = health;
        smokeEffect = (GameObject) Instantiate(smokePrefab, transform.position, transform.rotation);
        smokeEffect.SetActive(false);
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
        GameObject bulletObj = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); ;
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.SetBulletInfo(towerInfo);
        
        if (bullet != null) {
            bullet.Seek(target, PENETRATE_TARGET);
        }
    }

    void Update() {

        if (health <= 0) {
            print("Tower destroyed");
            smokeEffect.SetActive(true);
            return;
        }

        health -= Time.deltaTime * 4;
        print(health / maxHealth);
        healthBar.fillAmount = health / maxHealth;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower : Tower {

    private float rotationSpeed;
    private Transform rotationBase;
    private Transform firePoint;
    private float fireCountdown = 0f;
    private Transform target;

    private const bool PENETRATE_TARGET = false;

    public CannonTower(TowerInfo towerInfo) : base(towerInfo) {
        // GetComponentInChildren<Base>
        // use GetComponent to find base, firepoint
    }
    void Start() {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

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
        
        if (bullet != null) {
            bullet.Seek(target, PENETRATE_TARGET);
        }
    }
    void Update() {
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

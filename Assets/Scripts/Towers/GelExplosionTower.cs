using UnityEngine;

public class GelExplosionTower : Tower {

    private new const string ROTATION_BASE_NAME = "Gel_Explosive_Tower";
    private float rotationSpeed = 5f;
    private Transform rotationBase;
    private Transform firePoint;
    private float fireCountdown = 0f;
    private Transform target;
    private const bool PENETRATE_TARGET = false;

    [Header("Effect Prefabs")]
    public GameObject fireSoundEffect;
    private float fireSoundEffectCurrValue;
    private float fireSoundEffectTriggerPValue = 70;

    public override void SetTowerInfo(TowerInfo towerInfo) {
        base.SetTowerInfo(towerInfo);
        rotationBase = transform.Find(GelExplosionTower.ROTATION_BASE_NAME);
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

        fireSoundEffectCurrValue = Random.Range(0, 100);
        if (fireSoundEffect && fireSoundEffectCurrValue >= fireSoundEffectTriggerPValue) {
            GameObject fireSoundEffectPrefab = (GameObject)Instantiate(fireSoundEffect, firePoint.position, firePoint.rotation);
            fireSoundEffectPrefab.transform.SetParent(transform);
            Destroy(fireSoundEffectPrefab, 2f);
        }

        GameObject bulletObj = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); ;
        GelExplosionBullet bullet = bulletObj.GetComponent<GelExplosionBullet>();
        bullet.SetBulletInfo(towerInfo);
        bullet.SetImpactSoundActive(fireSoundEffectCurrValue >= fireSoundEffectTriggerPValue);

        if (bullet != null) {
            bullet.Seek(target, PENETRATE_TARGET);
        }
    }

    public override void Update() {
        base.Update();

        if (health <= 0) return;
        if (!target || isStopShooting) return;

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

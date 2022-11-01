using UnityEngine;

public class SpikeyTower : Tower {

    private float towerRadius = 1f;
    private int numBarrels = 8;
    private Transform[] targetTransforms;
    private Transform firePoint;
    private float fireCountdown = 0f;
    private bool isShooting;
    private const bool PENETRATE_TARGET = true;

    [Header("Effect Prefabs")]
    public GameObject shootingEffect;
    public GameObject shootingSound;
    private float shootEffectCurrValue;
    private float shootEffectTriggerPValue = 70;

    public override void SetTowerInfo(TowerInfo towerInfo) {
        base.SetTowerInfo(towerInfo);
        if (towerInfo.upgradeNum == 1) {
            this.numBarrels = 12;
        }

        firePoint = transform.Find(Tower.ROTATION_BASE_NAME).Find(Tower.FIRE_POINT_NAME);
        targetTransforms = GetTargetTransforms();
    }

    void Start() {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    /** Checks every second if any enemies are in range. */
    void UpdateTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(ENEMY_TAG);

        bool enemyFound = false;
        foreach (GameObject enemy in enemies) {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= range) {
                enemyFound = true;
                break;
            }
        }
        this.isShooting = enemyFound;
    }

    public override void Shoot() {
        base.Shoot();

        shootEffectCurrValue = Random.Range(0, 100);
        if (shootEffectCurrValue >= shootEffectTriggerPValue) {
            GameObject shootingEffectPrefab = (GameObject)Instantiate(shootingEffect, firePoint.position, firePoint.rotation);
            shootingEffectPrefab.transform.SetParent(transform);
            // GameObject shootingSoundPrefab = (GameObject)Instantiate(shootingSound, firePoint.position, firePoint.rotation);
            // shootingSoundPrefab.transform.SetParent(transform);
            Destroy(shootingEffectPrefab, 2f);
            // Destroy(shootingSoundPrefab, 1f);
        }

        foreach (var targetTransform in targetTransforms) {
            GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); ;
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.SetBulletInfo(towerInfo);

            if (bullet != null) {
                bullet.Seek(targetTransform, PENETRATE_TARGET);
            }
        }
    }

    public override void Update() {
        base.Update();
        if (health <= 0) return;

        if (isStopShooting) return;

        if (isShooting && fireCountdown <= 0f) {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    private Transform[] GetTargetTransforms() {
        Transform[] arrOfTransforms = new Transform[numBarrels];
        for (int bulletNumber = 0; bulletNumber < numBarrels; bulletNumber++) {
            var aimingPointPosition = firePoint.position;
            var angleOfFire = bulletNumber * (Mathf.PI / (numBarrels / 2));

            (float x, float z) = polarCoordinates(towerRadius, angleOfFire);

            aimingPointPosition.x += x;
            aimingPointPosition.z += z;

            Transform circleTarget = Instantiate(firePoint, aimingPointPosition, Quaternion.identity);
            circleTarget.name = "Target" + bulletNumber;
            circleTarget.parent = transform;
            arrOfTransforms[bulletNumber] = circleTarget;
        }

        return arrOfTransforms;
    }

    private (float, float) polarCoordinates(float radius, float angle) {
        float x = (float)(radius * Mathf.Cos(angle));
        float y = (float)(radius * Mathf.Sin(angle));

        return (x, y);
    }

}

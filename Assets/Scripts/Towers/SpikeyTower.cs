using UnityEngine;

public class SpikeyTower : Tower {

    private float rotationSpeed = 1f;
    private float towerRadius = 1f;
    private int numBarrels = 8;
    public Transform[] targetTransforms;
    public Transform firePoint;
    private float fireCountdown = 0f;
    private bool isShooting;

    private const bool PENETRATE_TARGET = true;

    public override void SetTowerInfo(TowerInfo towerInfo) {
        base.SetTowerInfo(towerInfo);
        firePoint = transform.Find(Tower.FIRE_POINT_NAME);
        targetTransforms = GetTargetTransforms();
    }

    void Start() {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    /** Checks every second if any enemies are in range. */
    void UpdateTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(ENEMY_TAG);

        foreach (GameObject enemy in enemies) {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= range) {
                this.isShooting = true;
                break;
            }
        }
    }

    public override void Shoot() {
        foreach (var targetTransform in targetTransforms) {
            GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); ;
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            
            if (bullet != null) {
                bullet.Seek(targetTransform, PENETRATE_TARGET);
            }
        }
    }

    private void Update() {
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

            GameObject circleTarget = GameObject.CreatePrimitive(PrimitiveType.Cube);
            circleTarget.transform.position = aimingPointPosition;
            circleTarget.transform.parent = transform;
            arrOfTransforms[bulletNumber] = circleTarget.transform;
        }

        return arrOfTransforms;
    } 
    
    private (float, float) polarCoordinates(float radius, float angle) {
        float x = (float) (radius * Mathf.Cos(angle));
        float y = (float) (radius * Mathf.Sin(angle));

        return (x, y);
    }
    
}

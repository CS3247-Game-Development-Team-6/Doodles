using UnityEngine;

public class Beyblade : Tower {
    private float spinningDetectRadius = 2.0f;
    private Collider[] enemyInRange;
    private Collider[] enemyInDamageRange;
    private const bool PENETRATE_TARGET = false;
    private bool isSpinning = false;
    public const string VFX_NAME = "VFX";
    private Transform rotationBase;
    private Transform firePoint;

    public override void SetTowerInfo(TowerInfo towerInfo) {
        base.SetTowerInfo(towerInfo);
        rotationBase = transform.Find(Tower.ROTATION_BASE_NAME);
        firePoint = rotationBase.Find(Tower.FIRE_POINT_NAME);
    }

    public void Start() {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        InvokeRepeating("Shoot", 0f, 0.5f);
    }

    public void UpdateTarget() {
        enemyInRange = Physics.OverlapSphere(transform.position, spinningDetectRadius);
    }

    public override void Shoot() {
        if (health <= 0) return;
        enemyInDamageRange = Physics.OverlapSphere(firePoint.position, range);
        foreach (var enemy in enemyInDamageRange) {
            if (enemy.gameObject.tag == ENEMY_TAG) {
                base.Shoot();
                GameObject bulletObj = (GameObject) Instantiate (
                    bulletPrefab, 
                    enemy.transform.position, 
                    enemy.transform.rotation
                );

                Bullet bullet = bulletObj.GetComponent<Bullet>();
                bullet.SetBulletInfo(towerInfo);
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                bullet.Seek(enemy.transform, PENETRATE_TARGET);
            }
        }
    }

    public override void Update() {
        base.Update();
        if (health <= 0) return;

        // check if enemyInRange contains gameobject tagged as enemy
        if (enemyInRange != null) {
            foreach (var enemy in enemyInRange) {
                if (enemy != null && enemy.gameObject.tag == ENEMY_TAG) {
                    isSpinning = true;
                    break;
                }
                isSpinning = false;
            }
        }

        if (isSpinning) {
            rotationBase.Rotate(0f, 360f * Time.deltaTime, 0f);
        }
    }

}
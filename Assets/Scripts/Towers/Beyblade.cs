using UnityEngine;

public class Beyblade : Tower {
    private Collider[] enemyInRange;
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
    }

    public void UpdateTarget() {
        enemyInRange = Physics.OverlapSphere(transform.position, range);
    }

    public override void Update() {
        // check if enemyInRange contains gameobject tagged as enemy
        if (enemyInRange != null) {
            foreach (var enemy in enemyInRange) {
                if (enemy.gameObject.tag == ENEMY_TAG) {
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
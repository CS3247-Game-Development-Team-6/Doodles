using UnityEngine;

public class Beyblade : Tower {
    private float spinningDetectRadius = 2.0f;
    private Collider[] enemyInRange;
    private Collider[] enemyInDamageRange;
    private const bool PENETRATE_TARGET = false;
    private bool isSpinning = false;
    private bool isEffectTriggered = false;
    public const string VFX_NAME = "VFX";
    private Transform rotationBase;
    private Transform firePoint;

    [Header("Effect Prefabs")]
    public GameObject spinningEffect;
    public GameObject spinningSound;
    protected GameObject spinningEffectParticle;
    protected GameObject spinningEffectAudio;

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

    // Trigger visual and sound effects
    private void TriggerEffect() {
        if (spinningEffectParticle == null && spinningEffectAudio == null) {
            spinningEffectParticle = (GameObject)Instantiate(spinningEffect, transform.position, transform.rotation);
            spinningEffectParticle.transform.parent = transform;
            spinningEffectAudio = (GameObject)Instantiate(spinningSound, transform.position, transform.rotation);
            spinningEffectAudio.transform.parent = transform;
            spinningEffectAudio.GetComponent<AudioSource>().Play();
        }

        Destroy(spinningEffectParticle, 10f);
        Destroy(spinningEffectAudio, 10f);
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
                isEffectTriggered = false;
            }
        }

        if (isSpinning) {
            rotationBase.Rotate(0f, 360f * Time.deltaTime, 0f);
            if (!isEffectTriggered) {
                isEffectTriggered = true;
                TriggerEffect();
            }
        }
    }

}
using UnityEngine;

public class LaserTower : Tower {
    private const string HIT_EFFECT_NAME = "HitEffect";

    private float rotationSpeed = 5f;
    private Transform rotationBase;
    private Transform firePoint;
    private Transform target;

    /**
     * Laser tower property: damage calculation, laser effect
     */
    private Enemy targetEnemy;
    private float targetTime;
    private ElementInfo elementInfo;
    private IEffectable targetEffectable;
    private int damage;
    private LineRenderer lineRenderer;
    private GameObject impactEffect;
    private Light impactLight;

    [Header("Effect Prefabs")]
    public GameObject fireSoundEffect;
    private float fireSoundEffectCurrTimeCount = 4;
    private float fireSoundEffectTriggerDuration = 5;

    public override void SetTowerInfo(TowerInfo towerInfo) {
        base.SetTowerInfo(towerInfo);
        rotationBase = transform.Find(Tower.ROTATION_BASE_NAME);
        firePoint = rotationBase.Find(Tower.FIRE_POINT_NAME);
        elementInfo = !towerInfo.element ? null : towerInfo.element;
        damage = towerInfo.damage;
    }

    public void Start() {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        lineRenderer = GetComponent<LineRenderer>();
        impactEffect = transform.Find(HIT_EFFECT_NAME).gameObject;
        impactLight = impactEffect.GetComponentInChildren<Light>();
    }

    void UpdateTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(ENEMY_TAG);

        float shortestDistance = Mathf.Infinity;

        GameObject nearestEnemy = null;
        target = null;
        targetEnemy = null;
        targetEffectable = null;

        foreach (GameObject enemy in enemies) {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance) {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= this.range) {
            target = nearestEnemy.transform;
            targetEnemy = target.GetComponent<Enemy>();
            targetEffectable = target.GetComponent<IEffectable>();
        }
    }

    public override void Update() {
        base.Update();

        if (health <= 0 || !target || isStopShooting) {
            if (lineRenderer.enabled) {
                lineRenderer.enabled = false;
                impactEffect.GetComponent<ParticleSystem>().Stop();
                impactLight.enabled = false;
                targetTime = 0;
            }
            return;
        }

        // Enemy target lock on 
        Vector3 dir = target.position - transform.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(rotationBase.rotation, lookAtRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        rotationBase.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        // laser damage
        targetTime += Time.deltaTime;
        targetEnemy.TakeDamage(damage * Mathf.Log(targetTime + 1), elementInfo); // +1 so that is positive
        targetEffectable.ApplyEffect(elementInfo ? elementInfo.effect : null);

        // decrease health
        base.DecreaseHealth(Time.deltaTime);

        // laser sound effect
        fireSoundEffectCurrTimeCount += Time.deltaTime;
        if (fireSoundEffectCurrTimeCount >= fireSoundEffectTriggerDuration) {
            fireSoundEffectCurrTimeCount = 0;
            GameObject fireSoundEffectPrefab = (GameObject)Instantiate(fireSoundEffect, firePoint.position, firePoint.rotation);
            fireSoundEffectPrefab.transform.SetParent(transform);
            Destroy(fireSoundEffectPrefab, 5f);
        }

        // laser graphics
        if (!lineRenderer.enabled) {
            lineRenderer.enabled = true;
            impactEffect.GetComponent<ParticleSystem>().Play();
            impactLight.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 impactDir = firePoint.position - target.position;
        impactEffect.transform.rotation = Quaternion.LookRotation(impactDir);
        impactEffect.transform.position = target.position + impactDir.normalized * .5f;

    }

}

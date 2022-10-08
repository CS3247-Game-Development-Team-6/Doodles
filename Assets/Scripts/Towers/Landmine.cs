using UnityEngine;

public class Landmine : Tower {
    private Collider[] enemyInRange;
    private const bool PENETRATE_TARGET = false;
    private const int ENEMY_LAYER = 6;
    private Transform rotationBase;
    private Transform firePoint;


    public override void SetTowerInfo(TowerInfo towerInfo) {
        base.SetTowerInfo(towerInfo);
        rotationBase = transform.Find(Tower.ROTATION_BASE_NAME);
        firePoint = rotationBase.Find(Tower.FIRE_POINT_NAME);
    }

    public void Start() {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        enemyInRange = Physics.OverlapSphere(transform.position, range, ENEMY_LAYER);
    }

    public void UpdateTarget() {
        enemyInRange = Physics.OverlapSphere(transform.position, range, ENEMY_LAYER);
    }

    public override void Shoot()
    {
        base.Shoot();
        foreach (var enemy in enemyInRange)
        {
            print(enemy.name);
            if (enemy.gameObject.tag == ENEMY_TAG) {
                GameObject bulletObj = (GameObject) Instantiate (
                    bulletPrefab, 
                    enemy.transform.position, 
                    enemy.transform.rotation
                );

                Bullet bullet = bulletObj.GetComponent<Bullet>();
                bullet.SetBulletInfo(towerInfo);
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                bullet.SetBulletDamage(calculateDamage(distance));
                bullet.Seek(enemy.transform, PENETRATE_TARGET);
            }
        }
        // TODO: Create tower self destruction effect
        // TODO: Create tower self destruction sound
        Destroy(gameObject);
    }

    private float calculateDamage(float distance) {
        float damage = Mathf.Cos((1/(range/3) * distance)) + 1; 
        return damage;
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();

        if (health <= 0) return;
        if (enemyInRange.Length == 0) return;

        // TODO: Create trigger for enemy when stepped on landmine
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3);
    }
}

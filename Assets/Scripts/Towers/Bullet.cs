using UnityEngine;

public class Bullet : MonoBehaviour {

    protected Transform target;
    public float speed = 70f;
    public float explosionRadius = 0f;
    public GameObject impactEffect;
    public GameObject impactSoundEffect;
    [SerializeField] private int bulletDamage;
    [SerializeField] private ElementEffectInfo _data;
    [SerializeField] private ElementInfo elementInfo;
    private bool isPassingThroughBullet;
    private bool isImpactSoundActive = false;

    public void SetBulletInfo(TowerInfo towerInfo) {
        this.speed = towerInfo.speed;
        this.explosionRadius = towerInfo.explosionRadius;
        this.bulletDamage = towerInfo.damage;
        this._data = !towerInfo.element ? null : towerInfo.element.effect;
        this.elementInfo = !towerInfo.element ? null : towerInfo.element;
        this.impactEffect = towerInfo.impactPrefab;
        this.impactSoundEffect = towerInfo.impactSound;
        this.isPassingThroughBullet = towerInfo.penetratesEnemy;
    }

    public float GetBulletDamage() {
        return bulletDamage;
    }

    public void SetBulletDamage(int damage) {
        this.bulletDamage = damage;
    }


    public float GetExplosionRadius() {
        return explosionRadius;
    }

    public void Seek(Transform _target, bool isPassing) {
        target = _target;
        isPassingThroughBullet = isPassing;
    }

    // Update is called once per frame
    protected virtual void Update() {
        if (target == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame) {
            HitTarget();
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    protected virtual void HitTarget(bool toDestroyThisFrame = true, Collider hitEnemy = null) {
        if (impactEffect) {
            GameObject impactEffectParticle = (GameObject)Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
            Destroy(impactEffectParticle, 2f);
        }

        if (isImpactSoundActive && impactSoundEffect) {
            GameObject impactSoundEffectParticle = (GameObject)Instantiate(impactSoundEffect, transform.position, transform.rotation);
            Destroy(impactSoundEffectParticle, 2f);
        }

        if (explosionRadius > 0f) {
            Explode();
        }

        if (target != null && target.CompareTag("Enemy")) {
            var effectable = target.GetComponent<IEffectable>();
            if (effectable != null) effectable.ApplyEffect(_data);
            target.GetComponent<Enemy>().TakeDamage(bulletDamage, elementInfo);
        }

        if (hitEnemy) {
            var effectable = hitEnemy.gameObject.GetComponent<IEffectable>();
            if (effectable != null) effectable.ApplyEffect(_data);
            hitEnemy.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage, elementInfo);
        }

        if (toDestroyThisFrame) {
            Destroy(gameObject);    // destroys the bullet
        }
    }

    protected virtual void Explode() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders) {
            if (collider.CompareTag("Enemy")) {
                var effectable = collider.GetComponent<IEffectable>();
                if (effectable != null) effectable.ApplyEffect(_data);
                collider.GetComponent<Enemy>().TakeDamage(bulletDamage, elementInfo);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            HitTarget(false, other);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public void SetImpactSoundActive(bool isActive) {
        isImpactSoundActive = isActive;
    }
}

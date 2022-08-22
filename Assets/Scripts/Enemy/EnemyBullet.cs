using UnityEngine;
using EZCameraShake;

// similar to tower bullet, but diff target
public class EnemyBullet : MonoBehaviour {
    private Transform target;
    private float speed;
    private int bulletDamage;

    public GameObject impactEffect;
    public GameObject damageText;

    public void Seek(Transform _target, float _speed, int damage) {
        target = _target;
        speed = _speed;
        bulletDamage = damage;
    }

    private void Update() {
        if (target == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // hit 
        if (dir.magnitude <= distanceThisFrame) {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    private void HitTarget() {
        if (gameObject.CompareTag("BossBullet")) {
            CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1f);
        }

        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);

        Damage(target);

        Destroy(gameObject);    // destroys the bullet
    }

    private void Damage(Transform _target) {
        if (_target.CompareTag("Player")) {
            _target.GetComponent<PlayerHealth>().TakeDamage(bulletDamage);
        }

        if (_target.CompareTag("Base")) {
            Base.receiveDmg(bulletDamage);
        }

        // show damage number
        DamageIndicator indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        indicator.SetDamageText(bulletDamage);
    }
}

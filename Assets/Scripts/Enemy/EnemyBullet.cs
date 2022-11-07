using UnityEngine;
using EZCameraShake;

// similar to tower bullet, but diff target
public class EnemyBullet : MonoBehaviour {
    private Transform target;
    private float speed;
    private int bulletDamage;
    private Map map;

    public GameObject impactEffect;
    public GameObject damageText;

    private void Start() {
        map = FindObjectOfType<Map>();
    }

    public virtual void Seek(Transform _target, float _speed, int damage) {
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
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);

        Damage(target);

        Destroy(gameObject);    // destroys the bullet
    }

    public virtual void Damage(Transform _target) {
        if (_target.CompareTag("Player")) {
            _target.GetComponent<PlayerHealth>().TakeDamage(bulletDamage);
        }

        if (_target.CompareTag("PlayerWall")) {
            _target.GetComponent<BarrierWall>().TakeDamage(bulletDamage);
        }

        if (_target.CompareTag("Base")) {
            if (_target.GetComponent<Base>() == null) {
                Debug.LogError($"Missing Base script on {_target}");
            }
            _target.GetComponent<Base>().TakeDmg(bulletDamage);
        }

        // show damage number
        DamageIndicator indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        indicator.SetDamageText(bulletDamage);
    }
}

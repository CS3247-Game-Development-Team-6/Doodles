using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

// similar to tower bullet, but diff target
public class EnemyBullet : MonoBehaviour
{
    private Transform target;
    public float speed = 70f;
    public GameObject impactEffect;

    private int bulletDamage;

    [SerializeField] private int initBulletDamage = 10;

    public void Start() {
        bulletDamage = initBulletDamage;
    }

    public void Seek(Transform _target) 
    { 
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) 
        { 
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        
        // hit 
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget() 
    {
        // Better implementation required to identify boss bullet
        if (bulletDamage >= 100)
        {
            // boss bullet
            CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1f);
        }

        GameObject effectIns = (GameObject) Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);

        Damage(target);

        Destroy(gameObject);    // destroys the bullet
    }

    void Damage(Transform _target) {
        if (_target.CompareTag("Player"))
        {
            _target.GetComponent<PlayerHealth>().TakeDamage(bulletDamage);
        }

        if (_target.CompareTag("Base"))
        {
            Base.receiveDmg(bulletDamage);
        }
    }

    public void ReduceBulletDamage(int _damage) {
        bulletDamage = initBulletDamage - _damage;
    }

    public void RestoreBulletDamage() {
        bulletDamage = initBulletDamage;
    }
}

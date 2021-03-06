using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Transform target;
    public float speed = 70f;
    public float explosionRadius = 0f;    
    public GameObject impactEffect;
    [SerializeField] private int bulletDamage;
    [SerializeField] private StatusEffectData _data;
    private bool isPassingThroughBullet = false;

    public int GetBulletDamage() {
        return bulletDamage;
    }

    public float GetExplosionRadius() {
        return explosionRadius;
    }

    public void Seek(Transform _target, bool isPassing) 
    { 
        target = _target;
        isPassingThroughBullet = isPassing;
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
        
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    void HitTarget(bool toDestroyThisFrame=true, Collider hitEnemy=null) 
    {
        GameObject impactEffectParticle = (GameObject) Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(impactEffectParticle, 5f);

        if (explosionRadius > 0f)
        {
            Explode();
        }

        if (target != null && target.CompareTag("Enemy"))
        {
            var effectable = target.GetComponent<IEffectable>();
            if (effectable != null) effectable.ApplyEffect(_data);

            target.GetComponent<Enemy>().TakeDamage(bulletDamage);
        }

        if (hitEnemy)
        {
            var effectable = hitEnemy.gameObject.GetComponent<IEffectable>();
            if (effectable != null) effectable.ApplyEffect(_data);
            hitEnemy.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
        }

        if (toDestroyThisFrame) 
            Destroy(gameObject);    // destroys the bullet
    }

    void Explode()
    {
        Collider[] colliders =  Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                var effectable = collider.GetComponent<IEffectable>();
                if (effectable != null) effectable.ApplyEffect(_data);

                collider.GetComponent<Enemy>().TakeDamage(bulletDamage);
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            HitTarget(false, other);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

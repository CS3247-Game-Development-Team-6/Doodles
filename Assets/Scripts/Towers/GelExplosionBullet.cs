using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GelExplosionBullet : Bullet {
    
    public float slowValue = 0.0f;

    public void SetSlowValue(float slowValue) {
        this.slowValue = slowValue;
    }
    
    void override HitTarget(bool toDestroyThisFrame = true, Collider hitEnemy = null) {
        if (impactEffect) {
            GameObject impactEffectParticle = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(impactEffectParticle, 5f);
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
            // TODO: Add slow; it conflicts with freezing - require a solution 
        }

        if (toDestroyThisFrame) {
            Destroy(gameObject);    // destroys the bullet
        }

        
    }

    void override Explode() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders) {
            if (collider.CompareTag("Enemy")) {
                var effectable = collider.GetComponent<IEffectable>();
                if (effectable != null) effectable.ApplyEffect(_data);
                collider.GetComponent<Enemy>().TakeDamage(bulletDamage, elementInfo);
                // TODO: Add slow; it conflicts with freezing - require a solution 
            }
        }

        
    }
    
}

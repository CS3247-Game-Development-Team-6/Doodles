using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GelExplosionBullet : Bullet {
    
    protected override void HitTarget(bool toDestroyThisFrame = true, Collider hitEnemy = null) {
        base.HitTarget(toDestroyThisFrame, hitEnemy);

        // apply gel to enemy
        if (hitEnemy) {
            var enemy = hitEnemy.gameObject.GetComponent<Enemy>();
            if (enemy != null) {
                enemy.ApplyEffect(new GelExplosionEffect());
            }
        }
    }

    protected override void Explode() {
        base.Explode();

        // apply gel in an area
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders) {
            if (collider.CompareTag("Enemy")) {
                var enemy = collider.gameObject.GetComponent<Enemy>();
                if (enemy != null) enemy.ApplyEffect(new GelExplosionEffect());
            }
        }
    }
}


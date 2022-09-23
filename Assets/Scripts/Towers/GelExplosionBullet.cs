using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GelExplosionBullet : Bullet {
    
    public float slowValue = 0.0f;

    public void SetSlowValue(float slowValue) {
        this.slowValue = slowValue;
    }
    
    public override void HitTarget(bool toDestroyThisFrame = true, Collider hitEnemy = null) {
        base.HitTarget(toDestroyThisFrame, hitEnemy);

        // TODO: Add slow; it conflicts with freezing - require a solution
    }

    public override void Explode() {
        base.Explode();

        // TODO: Add slow; it conflicts with freezing - require a solution
    }
    
}

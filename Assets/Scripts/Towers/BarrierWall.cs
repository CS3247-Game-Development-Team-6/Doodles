using UnityEngine;

public class BarrierWall : Tower {
    
    // private Transform rotationBase;
    // private Transform firePoint;

    public override void SetTowerInfo(TowerInfo towerInfo) {
        base.SetTowerInfo(towerInfo);
        // rotationBase = transform.Find(Tower.ROTATION_BASE_NAME);
        // firePoint = rotationBase.Find(Tower.FIRE_POINT_NAME);
    }

    public void Start() {
        
    }

    public override void Update() {
        
    }

    public void TakeDamage(int amount) {
        this.health -= amount;

        // float number between 0 and 1
        healthBar.fillAmount = this.health / maxHealth;

        if (this.health <= 0) {
            Destroy();
        }
    }

    public void Destroy() {
        Destroy(gameObject, 0.5f);
    }
}

using UnityEngine;

public class Landmine : Tower {
    private Transform rotationBase;
    private Transform firePoint;

    public override void SetTowerInfo(TowerInfo towerInfo) {
        base.SetTowerInfo(towerInfo);
        rotationBase = transform.Find(Tower.ROTATION_BASE_NAME);
        firePoint = rotationBase.Find(Tower.FIRE_POINT_NAME);
    }

    public void Start() {
        
    }

    // Update is called once per frame
    public override void Update() {
        
    }
}

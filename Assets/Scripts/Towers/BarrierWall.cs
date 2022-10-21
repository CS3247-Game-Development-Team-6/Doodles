public class BarrierWall : Tower, PlayerWall {

    public override void SetTowerInfo(TowerInfo towerInfo) {
        base.SetTowerInfo(towerInfo);
    }

    public override void DecreaseHealth(float amount) {
        base.DecreaseHealth(amount);

        if (base.IsDead()) {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int amount) {
        DecreaseHealth(amount);
    }

}

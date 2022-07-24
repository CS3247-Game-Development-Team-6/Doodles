using UnityEngine;

public class Tower : MonoBehaviour {

    public const string ENEMY_TAG = "Enemy";

    protected float range;
    protected float fireRate;
    protected int cost;
    protected int versionNum;
    protected ElementEffectInfo element;
    protected GameObject bulletPrefab;
    protected TowerInfo nextUpgrade;
    protected ElementInfo[] nextElements;

    protected Tower(TowerInfo towerInfo) {
        this.range = towerInfo.range;
        this.fireRate = towerInfo.fireRate;
        this.cost = towerInfo.cost;
        this.versionNum = towerInfo.versionNum;
        this.element = towerInfo.element;
        this.bulletPrefab = towerInfo.bulletPrefab;
        this.nextUpgrade = towerInfo.nextUpgrade;
        this.nextElements = towerInfo.nextElements;
    }

    /** Instantiates and fires bullets. */
    public virtual void Shoot() { }
}

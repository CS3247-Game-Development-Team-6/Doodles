using UnityEngine;

public class Tower : MonoBehaviour {

    public const string ENEMY_TAG = "Enemy";
    public const string ROTATION_BASE_NAME = "RotationBase";
    public const string FIRE_POINT_NAME = "FirePoint";

    protected string towerName;
    protected float range;
    protected float fireRate;
    protected int cost;
    protected int versionNum;
    protected ElementInfo element;
    protected GameObject bulletPrefab;
    protected TowerInfo nextUpgrade;
    protected ElementKeyValue[] nextElements;

    public virtual void SetTowerInfo(TowerInfo towerInfo) {
        this.towerName = towerInfo.towerName;
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

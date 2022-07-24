using UnityEngine;

public class Tower : MonoBehaviour {

    public const string ENEMY_TAG = "Enemy";
    public const string ROTATION_BASE_NAME = "RotationBase";
    public const string FIRE_POINT_NAME = "FirePoint";

    protected TowerInfo towerInfo;
    protected string towerName;
    protected float range;
    protected float fireRate;
    protected int cost;
    protected int versionNum;
    // Temporarily public for NodeUI, but eventually to replace with element in NodeUI
    public GameObject bulletPrefab { get; private set; }
    public ElementInfo element { get; private set; }
    public TowerInfo nextUpgrade { get; private set; }
    public ElementKeyValue[] nextElements { get; private set; }

    /** Set tower info from Node. */
    public virtual void SetTowerInfo(TowerInfo towerInfo) {
        this.towerInfo = towerInfo;
        this.towerName = towerInfo.towerName;
        this.range = towerInfo.range;
        this.fireRate = towerInfo.fireRate;
        this.cost = towerInfo.cost;
        this.versionNum = towerInfo.upgradeNum;
        this.element = towerInfo.element;
        this.bulletPrefab = towerInfo.bulletPrefab;
        this.nextUpgrade = towerInfo.nextUpgrade;
        this.nextElements = towerInfo.nextElements;
    }

    /** Instantiates and fires bullets. */
    public virtual void Shoot() { }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour {

    public const string ENEMY_TAG = "Enemy";
    public const string ROTATION_BASE_NAME = "RotationBase";
    public const string FIRE_POINT_NAME = "FirePoint";
    protected string towerName;
    protected float range;
    protected float fireRate;
    protected int cost;
    protected float health;
    protected float maxHealth;
    protected float healthDecayRate;
    protected int versionNum;
    protected Image healthBar;
    protected GameObject bulletPrefab;
    protected GameObject smokePrefab;
    public ElementInfo element { get; private set; }
    public TowerInfo towerInfo { get; protected set; }
    public TowerInfo nextUpgrade { get; private set; }
    public ElementKeyValue[] nextElements { get; private set; }
    public Dictionary<ElementType, TowerInfo> nextElement { get; private set; }

    /** Set tower info from Node. */
    public virtual void SetTowerInfo(TowerInfo towerInfo) {
        this.towerInfo = towerInfo;
        this.towerName = towerInfo.towerName;
        this.range = towerInfo.range;
        this.fireRate = towerInfo.fireRate;
        this.cost = towerInfo.cost;
        this.health = towerInfo.health;
        this.maxHealth = this.health;
        this.healthDecayRate = towerInfo.healthDecayRate;
        this.versionNum = towerInfo.upgradeNum;
        this.element = towerInfo.element;
        this.bulletPrefab = towerInfo.bulletPrefab;
        this.nextUpgrade = towerInfo.nextUpgrade;
        this.nextElement = new Dictionary<ElementType, TowerInfo>(3);
        foreach (ElementKeyValue pair in towerInfo.nextElements) {
            this.nextElement.Add(pair.element, pair.tower);
        }
        this.smokePrefab = (GameObject) Instantiate(towerInfo.smokePrefab, transform.position, transform.rotation);
        this.smokePrefab.GetComponent<ParticleSystem>().Stop();
        this.healthBar = transform.Find("TowerHealthCanvas/HealthBG/HealthBar").GetComponent<Image>();
    }

    /** Function accessable by enemy to damage tower. */
    public void DecreaseHealth(float amount) {
        health -= 50;
    }

    /** Instantiates and fires bullets. */
    public virtual void Shoot() {
        DecreaseHealth(healthDecayRate);
    }

    public virtual void Update() {
        if (health <= 0 && this.smokePrefab.GetComponent<ParticleSystem>().isStopped) {
            this.smokePrefab.GetComponent<ParticleSystem>().Play();
            return;
        }
        healthBar.fillAmount = health / maxHealth;
    }
}

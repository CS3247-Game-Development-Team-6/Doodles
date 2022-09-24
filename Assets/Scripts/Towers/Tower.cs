using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour {

    // Tower info
    protected string towerName;
    protected int versionNum;

    // Macros
    public const string ENEMY_TAG = "Enemy";
    public const string ROTATION_BASE_NAME = "RotationBase";
    public const string FIRE_POINT_NAME = "FirePoint";

    // Tower Attack Attributes
    protected float range;
    protected float fireRate;
    protected int cost;

    // Tower Durability Attributes
    protected float health;
    protected float maxHealth;
    protected float healthDecayRate;

    // Prefabs
    protected GameObject bulletPrefab;
    protected GameObject smokePrefab;
    protected AudioSource damagedSound;

    // Image
    protected Image healthBar;

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

        // Prepare Smoke Effect to be played
        this.smokePrefab = (GameObject) Instantiate(towerInfo.smokePrefab, transform.position, transform.rotation);
        this.smokePrefab.GetComponent<ParticleSystem>().Stop();

        // Create health bar UI for each tower
        GameObject healthBarPrefab = Instantiate(towerInfo.healthBarPrefab, transform.position, transform.rotation);
        healthBarPrefab.transform.SetParent(transform);
        healthBarPrefab.transform.position = new Vector3( // hard coded offset
            healthBarPrefab.transform.position.x, 
            healthBarPrefab.transform.position.y + 1.3f, 
            healthBarPrefab.transform.position.z + 0.5f);
        healthBarPrefab.transform.rotation = Quaternion.Euler(50, 0, 0);
        this.healthBar = healthBarPrefab.transform.Find("HealthBG/HealthBar").GetComponent<Image>();
        
        // Prepare audio to be played
        GameObject damagedSoundPrefab = Instantiate(towerInfo.damagedSoundPrefab, transform.position, transform.rotation);
        damagedSoundPrefab.transform.SetParent(transform);
        this.damagedSound = damagedSoundPrefab.GetComponent<AudioSource>();
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
            this.damagedSound.Play();
            return;
        }
        healthBar.fillAmount = health / maxHealth;
    }
}

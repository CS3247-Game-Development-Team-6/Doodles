using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour {

    // Tower info
    protected string towerName;
    protected int versionNum;
    protected int cost;
    protected int damageFixCost;

    // Macros
    public const string ENEMY_TAG = "Enemy";
    public const string ROTATION_BASE_NAME = "RotationBase";
    public const string FIRE_POINT_NAME = "FirePoint";

    // Tower Attack Attributes
    protected float range;
    protected float fireRate;

    // Tower Durability Attributes
    protected float health;
    protected float maxHealth;
    protected float healthDecayRate;
    private bool damageEffectPlayed = false;

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
        this.damageFixCost = towerInfo.damageFixCost;
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
        health -= amount;
    }

    /** Function to increase tower health. */
    public void IncreaseHealth(float amount) {
        health += amount;
        if (health > maxHealth) {
            health = maxHealth;
        }
    }

    /** Function to increase tower health. */
    public bool restoreHealth() {
        IncreaseHealth(maxHealth);
        return true;
    }

    /** Instantiates and fires bullets. */
    public virtual void Shoot() {
        DecreaseHealth(healthDecayRate);
    }

    /** Check if tower is dead. */
    public bool IsDead() {
        return health <= 0;
    }

    public virtual void Update() {
        healthBar.fillAmount = health / maxHealth;
        if (health <= 0 && !damageEffectPlayed) {
            this.smokePrefab.GetComponent<ParticleSystem>().Play();
            this.damagedSound.Play();
            damageEffectPlayed = true;
            return;
        } else if (health > 0 && damageEffectPlayed) {
            this.smokePrefab.GetComponent<ParticleSystem>().Stop();
            this.damagedSound.Stop();
            damageEffectPlayed = false;
            return;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TowerEffects))]
public class Tower : MonoBehaviour {

    // Tower info
    protected string towerName;
    protected int versionNum;
    protected int cost;
    protected float damageFixFactor;
    public float damageFixCost;

    // Macros
    public const string ENEMY_TAG = "Enemy";
    public const string ROTATION_BASE_NAME = "RotationBase";
    public const string FIRE_POINT_NAME = "FirePoint";
    public const string TOWER_DAMAGE_EFFECT_NAME = "damage";

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
    protected GameObject damageEffectPrefab;

    // Image
    protected Image healthBar;

    // Tower effects
    private TowerEffects towerEffectsManager;
    protected bool stoppable;
    protected bool isStopShooting;

    public ElementInfo element { get; private set; }
    public TowerInfo towerInfo { get; protected set; }
    public TowerInfo nextUpgrade { get; private set; }
    public ElementKeyValue[] nextElements { get; private set; }
    public Dictionary<ElementType, TowerInfo> nextElement { get; private set; }
    public List<CellType> allowedCellTypes { get; private set; }

    private void Awake() {
        towerEffectsManager = GetComponent<TowerEffects>();
        isStopShooting = false;
    }

    /** Set tower info from Node. */
    public virtual void SetTowerInfo(TowerInfo towerInfo) {
        this.towerInfo = towerInfo;
        this.towerName = towerInfo.towerName;
        this.range = towerInfo.range;
        this.fireRate = towerInfo.fireRate;
        this.cost = towerInfo.cost;
        this.damageFixCost = 0;
        this.damageFixFactor = towerInfo.damageFixFactor;
        this.health = towerInfo.health;
        this.maxHealth = this.health;
        this.healthDecayRate = towerInfo.healthDecayRate;
        this.versionNum = towerInfo.upgradeNum;
        this.element = towerInfo.element;
        this.bulletPrefab = towerInfo.bulletPrefab;
        this.nextUpgrade = towerInfo.nextUpgrade;
        this.nextElement = new Dictionary<ElementType, TowerInfo>(3);
        this.allowedCellTypes = towerInfo.allowedCellTypes;
        this.stoppable = towerInfo.stoppable;
        foreach (ElementKeyValue pair in towerInfo.nextElements) {
            this.nextElement.Add(pair.element, pair.tower);
        }

        // Prepare Damage Effect to be played
        this.damageEffectPrefab = Instantiate(TowerManager.instance.GetEffectPrefab(TOWER_DAMAGE_EFFECT_NAME), transform.position, transform.rotation);
        this.damageEffectPrefab.transform.SetParent(transform);
        this.damageEffectPrefab.GetComponentInChildren<ParticleSystem>().Stop();

        // Create health bar UI for each tower
        GameObject healthBarPrefab = Instantiate(TowerManager.instance.GetHealthBarPrefab(), transform.position, transform.rotation);
        healthBarPrefab.transform.SetParent(transform);
        healthBarPrefab.transform.position = new Vector3( // hard coded offset
            healthBarPrefab.transform.position.x,
            healthBarPrefab.transform.position.y + 1.3f,
            healthBarPrefab.transform.position.z + 0.5f);
        healthBarPrefab.transform.rotation = Quaternion.Euler(50, 0, 0);
        this.healthBar = healthBarPrefab.transform.Find("HealthBG/HealthBar").GetComponent<Image>();
    }

    /** Function accessable by enemy to damage tower. */
    public virtual void DecreaseHealth(float amount) {
        health -= amount;
    }

    /** Function to check if tower is full health. */
    public bool IsFullHealth() {
        return health == maxHealth;
    }

    /** Function to increase tower health. */
    public void IncreaseHealth(float amount) {
        health += amount;
        if (health > maxHealth) {
            health = maxHealth;
        }
        TowerManager.instance.SpawnEffect("fix", transform);
    }

    /** Function to increase tower health. */
    public bool RestoreHealth() {
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

    /** Function to update damageFixCost. */
    public void UpdateDamageFixCost() {
        float healthLoss = maxHealth - health;
        damageFixCost = Mathf.FloorToInt(damageFixFactor * healthLoss);
    }

    public virtual void Update() {
        healthBar.fillAmount = health / maxHealth;
        UpdateDamageFixCost();
        if (health <= 0 && !damageEffectPlayed) {
            this.damageEffectPrefab.GetComponent<AudioSource>().Play();
            this.damageEffectPrefab.GetComponentInChildren<ParticleSystem>().Play();
            damageEffectPlayed = true;
        } else if (health > 0 && damageEffectPlayed) {
            this.damageEffectPrefab.GetComponent<AudioSource>().Stop();
            this.damageEffectPrefab.GetComponentInChildren<ParticleSystem>().Stop();
            damageEffectPlayed = false;
        }
    }

    public void ApplyEffect(ITowerEffect effect) {
        StartCoroutine(towerEffectsManager.HandleEffect(effect));
    }

    public bool SetStopShooting(bool boo) {
        if (stoppable) isStopShooting = boo;
        return isStopShooting;
    }
}

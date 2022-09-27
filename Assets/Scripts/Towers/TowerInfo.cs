using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElementKeyValue {
    public ElementType element;
    public TowerInfo tower;
}

[CreateAssetMenu(fileName = "TowerInfo", menuName = "Tower Type")]
public class TowerInfo : ScriptableObject {

    public const int MAX_RANGE = 15;
    public const int MAX_UPGRADES = 1;
    public const float DMG_MULTIIPLIER = 1.2f;

    [Header("Stats")]
    public string towerName;
    [TextArea(4,8)] public string towerDesc;

    /** Range of detection and shooting. */
    [Range(0, MAX_RANGE)] public float range;
    /** upgradeNum corresponds to the number of upgrades this tower has had. */
    [Range(0, MAX_UPGRADES)] public int upgradeNum;
    /** Number of rounds fired per second. */
    [Range(0, Mathf.Infinity)] public float fireRate;
    /** element corresponds to the element of this tower (if any). */
    public ElementInfo element;
    /** cost is the additional cost to create this tower from previous version. */
    public int cost;
    /** the celltypes this tower can be built on. */
    public List<CellType> allowedCellTypes;

    [Header("Bullet")]
    /** Speed of bullet per second. */
    [Range(1, Mathf.Infinity)] public float speed;
    /** Explosion radius of bullet AOE effect. */
    [Range(0, Mathf.Infinity)] public float explosionRadius;
    /** Damage of bullet on hitting enemy. */
    public int damage;
    /** Is destroyed on hit, or penetrates through enemy. */
    public bool penetratesEnemy;
    /** Particle effect on collision with enemy. */
    public GameObject impactPrefab;

    /** TODO: Add bullet information (e.g. damage etc.) which will be passed
     *  to Bullet class on initialization. */

    [Header("Prefabs")]
    public GameObject towerPrefab;
    public GameObject bulletPrefab;

    [Header("Upgrades")]
    /** Link to the info of the next upgrade. */
    public TowerInfo nextUpgrade;
    /** Links to the info of the elemental versions. */
    public ElementKeyValue[] nextElements;

    /** DEV ONLY: Autofills upgrade with details based on this towerInfo. 
     * NEVER use in production code, only use to quickly autoset details in the Upgrade TowerInfos.
     * DOES NOT set towerName or any of the prefabs or linking upgrades.
     */
    public void AutoFillUpgradeInfo() {
        if (!this.nextUpgrade) return;

        this.nextUpgrade.upgradeNum = this.upgradeNum + 1;
        this.nextUpgrade.damage = Mathf.FloorToInt((float)this.damage * DMG_MULTIIPLIER);

        this.nextUpgrade.element = this.element;
        this.nextUpgrade.speed = this.speed;
        this.nextUpgrade.explosionRadius = this.explosionRadius;
        this.nextUpgrade.range = this.range;
        this.nextUpgrade.fireRate = this.fireRate;
        this.nextUpgrade.penetratesEnemy = this.penetratesEnemy;
    } 

}

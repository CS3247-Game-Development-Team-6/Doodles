using UnityEngine;

[System.Serializable]
public class ElementInfo {
    public ElementEffectInfo element;
    public TowerInfo elementTowerInfo;
}

[CreateAssetMenu(fileName = "TowerInfo", menuName = "Tower Type")]
public class TowerInfo : ScriptableObject {

    public const int MAX_RANGE = 15;
    public const int MAX_UPGRADES = 1;

    [Header("Stats")]
    public string towerName;
    /** Range of detection and shooting. */
    [Range(0, MAX_RANGE)] public float range;
    /** versionNum corresponds to the number of upgrades this tower has had. */
    [Range(0, MAX_UPGRADES)] public int versionNum;
    /** Number of rounds fired per second. */
    [Range(0, Mathf.Infinity)] public float fireRate;
    /** element corresponds to the element of this tower (if any). */
    public ElementEffectInfo element;
    /** cost is the additional cost to create this tower from previous version. */
    public int cost;

    /** TODO: Add bullet information (e.g. damage etc.) which will be passed
     *  to Bullet class on initialization. */

    [Header("Prefabs")]
    public GameObject towerPrefab;
    public GameObject bulletPrefab;

    [Header("Upgrades")]
    /** Link to the info of the next upgrade. */
    public TowerInfo nextUpgrade;
    /** Links to the info of the elemental versions. */
    public ElementInfo[] nextElements;

}

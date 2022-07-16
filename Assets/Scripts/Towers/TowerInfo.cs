using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElementCost {
    public ElementEffectInfo element;
    public int cost;
}

[CreateAssetMenu(fileName = "TowerInfo", menuName = "Tower Type")]
public class TowerInfo : ScriptableObject {

    public const int MAX_RANGE = 15;

    [Header("Tower attributes")]

    public string name;
    /** Range of detection and shooting. */
    [Range(0, MAX_RANGE)] public float range;
    /** Speed the base rotates at. */
    [Range(0, Mathf.Infinity)] public float rotationSpeed;
    /** Costs of each version. */
    [Tooltip("Costs of each upgrade (index is the number of upgrades, 0 being the base)")]
    public int[] costs;
    /** Costs of each element. */
    [Tooltip("Costs of each element (key is the element's ScriptableObject)")]
    public ElementCost[] elementCosts;

    [Header("Tower assets")]

    public GameObject towerPrefab;
    public GameObject bulletPrefab;

}

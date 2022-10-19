using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DontDestroy))]
public class LoadoutContainer : MonoBehaviour {
    public List<TowerInfo> towersToLoad;
    public GameObject shopSlotPrefab;
}

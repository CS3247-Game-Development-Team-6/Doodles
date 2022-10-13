using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

/* deprecated: use CardManager
 */
public class ShopTowerUI : MonoBehaviour, IComparable<ShopTowerUI> {
    public int Index { get; set; } = -1;
    public TowerInfo towerInfo;
    public Image image;
    public TextMeshProUGUI cost;
    public Sprite selectedBg;
    public Sprite unselectedBg;
    private Shop shop;

    private void Start() {
        shop = GetComponentInParent<Shop>();
        cost.text = "";
    }


    private void Update() {
        if (towerInfo) {
            image.sprite = towerInfo.sprite;
            image.color = Color.white;
            cost.text = towerInfo.cost.ToString();
        } else {
            image.sprite = null;
            image.color = Color.clear;
            cost.text = "";
        }
    }

    public void SetTowerInShop() {
        if (!shop) {
            Debug.LogError("Parent of this button is not a shop.");
            return;
        } else if (!towerInfo) {
            Debug.LogError("Tower not set for this button.");
            return;
        }

        // shop.SetTowerToBuild(this);
    }

    public int CompareTo(ShopTowerUI other) {
        return Index - other.Index;
    }
}

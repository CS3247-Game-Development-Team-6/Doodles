using UnityEngine;

public class ShopTowerUI : MonoBehaviour {
    public TowerInfo towerInfo;
    public Sprite selected;
    public Sprite unselected;
    private Shop shop;

    private void Start() {
        shop = GetComponentInParent<Shop>();
    }

    public void SetTowerInShop() {
        if (!shop) {
            Debug.LogError("Parent of this button is not a shop.");
            return;
        } else if (!towerInfo) {
            Debug.LogError("Tower not set for this button.");
            return;
        }

        shop.SetTowerToBuild(this);
    }
}

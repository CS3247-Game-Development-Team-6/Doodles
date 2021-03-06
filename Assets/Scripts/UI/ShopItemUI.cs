using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public Button button;
    public GameObject tower;
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
        } else if (!tower) {
            Debug.LogError("Tower not set for this button.");
            return;
        }

        shop.SetTowerAttempt(this);

    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class CardManager : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

    /**
     * loadout tower selection
     */
    private GameObject selfGO;
    private TowerSelectionManager towerSelectionManager;

    /**
     * in-game tower building
     */
    public TowerInfo towerInfo;
    public Sprite selected;
    public Sprite unselected;
    private Shop shop;

    void Start() {
        shop = GetComponentInParent<Shop>();
    }

    public void InitCard(GameObject _selfGO, TowerSelectionManager _towerSelectionManager) {
        selfGO = _selfGO;
        towerSelectionManager = _towerSelectionManager;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (TowerSelectionManager.isInGame) {
            SetTowerInShop();
        } else {
            towerSelectionManager.SelectTower(selfGO);
        }

    }

    public void OnPointerEnter(PointerEventData eventData) {
        towerSelectionManager.HoverTower(towerInfo, selected);
    }

    public void OnPointerExit(PointerEventData eventData) {
        towerSelectionManager.UnhoverTower();
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

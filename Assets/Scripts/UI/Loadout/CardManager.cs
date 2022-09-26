using UnityEngine;
using UnityEngine.EventSystems;

public class CardManager : MonoBehaviour, IPointerDownHandler {
    public GameObject selfGO;
    public TowerSelectionManager towerSelectionManager;
    public CardManager parentCard;

    void Start() {
    }

    void Update() {

    }

    public void OnPointerDown(PointerEventData eventData) {
        if (TowerSelectionManager.isInGame) {
            GetComponent<ShopTowerUI>().SetTowerInShop();
        } else {
            towerSelectionManager.SelectTower(selfGO, this);
        }

    }

}

using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    [SerializeField] private ShopTowerUI selectedTower;
    [SerializeField] private ParticleSystem invalidAction;
    [SerializeField] PlayerMovement playerMovement;
    private TowerManager towerManager;

    private void Start() {
        towerManager = TowerManager.instance;
        selectedTower = GetComponentInChildren<ShopTowerUI>();
        if (selectedTower) {
            SetTowerToBuild(selectedTower);
        }
    }

    private void TriggerInvalidAction() {
        Instantiate(invalidAction, transform);
    }

    public void SetTowerToBuild(ShopTowerUI item) {
        if (playerMovement && playerMovement.GetIsBuilding()) {
            TriggerInvalidAction();
            return;
        }
        item.gameObject.GetComponent<Image>().sprite = item.selected;
        if (selectedTower != null && selectedTower != item) { 
            selectedTower.gameObject.GetComponent<Image>().sprite = selectedTower.unselected;
        }

        selectedTower = item;
        TowerInfo towerInfo = item.towerInfo;
        towerManager.SetTowerToBuild(towerInfo);
    }
}

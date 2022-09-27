using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    [SerializeField] private CardManager selectedTower;
    [SerializeField] private ParticleSystem invalidAction;
    [SerializeField] PlayerMovement playerMovement;

    private void TriggerInvalidAction() {
        Instantiate(invalidAction, transform);
    }

    public void SetTowerToBuild(CardManager item) {
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
        TowerManager.instance.SetTowerToBuild(towerInfo);
    }
}

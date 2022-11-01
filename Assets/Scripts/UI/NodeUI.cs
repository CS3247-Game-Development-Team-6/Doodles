using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NodeButtonInfo {
    public GameObject button { get; private set; }
    public Image image { get; private set; }
    public NodeUITooltipTrigger tooltip { get; private set; }
    public Sprite defaultSprite;
    public Sprite activeSprite;
    public Sprite disabledSprite;
    

    public void Setup(GameObject button) {
        this.button = button;
        this.image = button.GetComponent<Image>();
        this.tooltip = button.GetComponent<NodeUITooltipTrigger>();
    }
}

public class NodeUI : MonoBehaviour {
    public GameObject ui;
    public GameObject playerGO;
    private Node selectedNode;

    public float zOffsetMultiplier;
    public float xOffsetMultiplier;
    public float xOffsetUpperShift;
    private float xOffset;
    private float zOffset;

    public NodeButtonInfo fire;
    public NodeButtonInfo ice;
    public NodeButtonInfo water;
    public NodeButtonInfo upgrade;
    public NodeButtonInfo destroy;
    public NodeButtonInfo fix;
    private Dictionary<ElementType, NodeButtonInfo> elementButtonInfos;

    private void Start() {
        Transform canvas = transform.Find("Canvas");
        Transform othersGroup = canvas.Find("Buttons");
        Transform elementsGroup = canvas.Find("Elements");
        this.upgrade.Setup(othersGroup.Find("Upgrade").gameObject);
        this.destroy.Setup(othersGroup.Find("Destroy").gameObject);
        this.fix.Setup(othersGroup.Find("Fix").gameObject);
        this.fire.Setup(elementsGroup.Find("Fire").gameObject);
        this.ice.Setup(elementsGroup.Find("Ice").gameObject);
        this.water.Setup(elementsGroup.Find("Water").gameObject);
        elementButtonInfos = new Dictionary<ElementType, NodeButtonInfo>(3);
        elementButtonInfos.Add(ElementType.FIRE, this.fire);
        elementButtonInfos.Add(ElementType.ICE, this.ice);
        elementButtonInfos.Add(ElementType.WATER, this.water);
    }

    private void FixedUpdate() {
        // Right clicking away when NodeUI is on now deactivates the NodeUI
        if (Input.GetMouseButtonDown(1) && ui.activeSelf) {
            ui.SetActive(false);
        }
    }

    private void Update() {
        if (ui.activeSelf) {
            if (IsPlayerTooFar())
                ui.SetActive(false);
        } else {
            TooltipSystem.Hide();
        }
    }

    public void SetTarget(Node target)
    {
        /* Node UI location is dynamically set, with some hardcoded values here. */
        selectedNode = target;
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(target.transform.position);
        xOffset = (0.5f - screenPoint.x / Camera.main.pixelWidth) * xOffsetMultiplier;

        // Bottom left and right corners
        if (screenPoint.y / Camera.main.pixelHeight < 0.2 && 
            Mathf.Abs(0.5f -  screenPoint.x / Camera.main.pixelWidth) > 0.28) {
            zOffset = 0.01f;
            xOffset *= 2f;
        // Middle of map (on y axis)
        } else if (screenPoint.y / Camera.main.pixelHeight < 0.6) {
            zOffset = 0.03f;
        // Top third of map
        } else {
            zOffset = (0.5f - screenPoint.y / Camera.main.pixelHeight) * zOffsetMultiplier;
            xOffset += xOffset < 0 ? (-1 * xOffsetUpperShift) : xOffsetUpperShift;
        }

        transform.position = selectedNode.towerBuildPosition + new Vector3(xOffset, 0, zOffset);

        if (selectedNode.HasTower()) {

            upgrade.image.sprite = selectedNode.isUpgraded ? upgrade.disabledSprite : upgrade.defaultSprite;
            upgrade.tooltip.isNotAvailable = selectedNode.isUpgraded;
            upgrade.tooltip.SetTowerInfo(selectedNode.tower.towerInfo, selectedNode.tower.nextUpgrade);
            
            fix.tooltip.isFixButton = true;
            fix.tooltip.SetTowerInfo(selectedNode.tower.towerInfo, selectedNode.tower.nextUpgrade, selectedNode.tower);
            
            TowerInfo currTower = selectedNode.tower.towerInfo;

            if (!selectedNode.tower.element) {
                foreach (var pair in elementButtonInfos) {
                    NodeButtonInfo button = pair.Value;
                    button.image.sprite = button.defaultSprite;
                    button.tooltip.SetTowerInfo(selectedNode.tower.towerInfo, selectedNode.tower.nextElement[pair.Key]);
                    button.tooltip.isNotAvailable = false;
                }
            } else {
                ElementType element = selectedNode.tower.element.type;
                foreach (var pair in elementButtonInfos) {
                    NodeButtonInfo button = pair.Value;
                    button.image.sprite = pair.Key == element ? button.activeSprite : button.disabledSprite;
                    button.tooltip.isNotAvailable = true;
                }
            }
            ui.SetActive(true);
        }
    }

    public void Hide() {
        ui.SetActive (false);
    }

    public bool IsPlayerTooFar() {
        float maxDistance = playerGO.GetComponent<PlayerMovement>().GetBuildDistance();
        return (selectedNode.transform.position - playerGO.transform.position).magnitude > maxDistance;
    }
}

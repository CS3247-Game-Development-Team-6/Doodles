using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public GameObject ui;
    public Node selectedNode;
    public GameObject playerGO;

    public float zOffsetMultiplier;
    public float xOffsetMultiplier;
    public float xOffsetUpperShift;
    private float xOffset;
    private float zOffset;
    public Sprite fireDefault;
    public Sprite fireActive;
    public Sprite iceDefault;
    public Sprite iceActive;
    public Sprite waterDefault;
    public Sprite waterActive;
    public Sprite towerRadius;
    public Sprite missleRadius;
    public Sprite iceDisable;
    public Sprite waterDisable;
    public Sprite fireDisable;
    public Sprite upgradeDisable;
    public Sprite upgradeDefault;

    private GameObject upgradeButton;
    private GameObject destroyButton;
    private GameObject fireButton;
    private GameObject iceButton;
    private GameObject waterButton;

    private void Start() {
        Transform canvas = transform.Find("Canvas");
        Transform othersGroup = canvas.Find("Buttons");
        Transform elementsGroup = canvas.Find("Elements");
        this.upgradeButton = othersGroup.Find("Upgrade").gameObject;
        this.destroyButton = othersGroup.Find("Destroy").gameObject;
        this.fireButton = elementsGroup.Find("Fire").gameObject;
        this.iceButton = elementsGroup.Find("Ice").gameObject;
        this.waterButton = elementsGroup.Find("Water").gameObject;
    }

    private void FixedUpdate() {
        // Right clicking away when NodeUI is on now deactivates the NodeUI
        if (Input.GetMouseButtonDown(1) && ui.activeSelf) {
            ui.SetActive (false);
        }
    }

    private Image GetButtonImage(GameObject buttonObj) {
        Button button = buttonObj.GetComponent<Button>();
        if (!button) return null;
        return button.GetComponent<Image>();
    }

    private NodeUITooltipTrigger GetTooltip(GameObject buttonObj) {
        return buttonObj.GetComponent<NodeUITooltipTrigger>();
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

        transform.position = selectedNode.GetTowerBuildPosition() + new Vector3(xOffset, 0, zOffset);

        if (selectedNode.GetIsTowerBuilt()) {
            ui.SetActive(true);
            Image fireImage = GetButtonImage(fireButton);
            Image iceImage = GetButtonImage(iceButton);
            Image waterImage = GetButtonImage(waterButton);
            Image upgradeImage = GetButtonImage(upgradeButton);
            NodeUITooltipTrigger fireTooltip = GetTooltip(fireButton);

            upgradeImage.sprite = selectedNode.GetIsUpgraded() ? upgradeDisable : upgradeDefault;

            if (!selectedNode.tower.element) {
                fireImage.sprite = fireDefault;
                iceImage.sprite = iceDefault;
                waterImage.sprite = waterDefault;
            } else {
                ElementType element = selectedNode.tower.element.type;
                if (element == ElementType.FIRE) {
                    fireImage.sprite = fireActive;
                    iceImage.sprite = iceDisable;
                    waterImage.sprite = waterDisable;
                } else if (element == ElementType.WATER) {
                    fireImage.sprite = fireDisable;
                    iceImage.sprite = iceDisable;
                    waterImage.sprite = waterActive;
                } else if (element == ElementType.ICE) {
                    fireImage.sprite = fireDisable;
                    iceImage.sprite = iceActive;
                    waterImage.sprite = waterDisable;
                }
            }
        }
    }

    public void Hide() {
        ui.SetActive (false);
    }
}

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
    // To use in future to access selectedNode
    private TowerManager towerManager;
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

    private void Start() {
        towerManager = TowerManager.instance;
    }

    private void FixedUpdate() {
        // Right clicking away when NodeUI is on now deactivates the NodeUI
        if (Input.GetMouseButtonDown(1) && ui.activeSelf) {
            ui.SetActive (false);
        }
    }

    public void SetTarget(Node _target)
    {
        /* Node UI location is dynamically set, with some hardcoded values here. */
        selectedNode = _target;
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(_target.transform.position);
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

        if (selectedNode.GetIsTowerBuilt())
        {
            ui.SetActive(true);


            if (selectedNode.towerObj.GetComponent<Tower>().bulletPrefab.tag == "Fire") {
                Button fireButton = GameObject.Find("/NodeUI/Canvas/Elements/Fire").GetComponent<Button>();
                Button iceButton = GameObject.Find("/NodeUI/Canvas/Elements/Ice").GetComponent<Button>();
                Button waterButton = GameObject.Find("/NodeUI/Canvas/Elements/Water").GetComponent<Button>();

                fireButton.GetComponent<Image>().sprite = fireActive;
                iceButton.GetComponent<Image>().sprite = iceDisable;
                waterButton.GetComponent<Image>().sprite = waterDisable;

            } else if (selectedNode.towerObj.GetComponent<Tower>().bulletPrefab.tag == "Ice") {
                Button fireButton = GameObject.Find("/NodeUI/Canvas/Elements/Fire").GetComponent<Button>();
                Button iceButton = GameObject.Find("/NodeUI/Canvas/Elements/Ice").GetComponent<Button>();
                Button waterButton = GameObject.Find("/NodeUI/Canvas/Elements/Water").GetComponent<Button>();

                fireButton.GetComponent<Image>().sprite = fireDisable;
                iceButton.GetComponent<Image>().sprite = iceActive;
                waterButton.GetComponent<Image>().sprite = waterDisable;
            } else if (selectedNode.towerObj.GetComponent<Tower>().bulletPrefab.tag == "Water") {
                Button fireButton = GameObject.Find("/NodeUI/Canvas/Elements/Fire").GetComponent<Button>();
                Button iceButton = GameObject.Find("/NodeUI/Canvas/Elements/Ice").GetComponent<Button>();
                Button waterButton = GameObject.Find("/NodeUI/Canvas/Elements/Water").GetComponent<Button>();

                fireButton.GetComponent<Image>().sprite = fireDisable;
                iceButton.GetComponent<Image>().sprite = iceDisable;
                waterButton.GetComponent<Image>().sprite = waterActive;
            } else {
                Button fireButton = GameObject.Find("/NodeUI/Canvas/Elements/Fire").GetComponent<Button>();
                Button iceButton = GameObject.Find("/NodeUI/Canvas/Elements/Ice").GetComponent<Button>();
                Button waterButton = GameObject.Find("/NodeUI/Canvas/Elements/Water").GetComponent<Button>();

                fireButton.GetComponent<Image>().sprite = fireDefault;
                iceButton.GetComponent<Image>().sprite = iceDefault;
                waterButton.GetComponent<Image>().sprite = waterDefault;
            }

            if (selectedNode.towerObj.tag == "Missile") {
                Image attackRadius = GameObject.Find("/NodeUI/Canvas/RadiusCanvas/AttackRadius").GetComponent<Image>();
                attackRadius.sprite = missleRadius;
            } else if (selectedNode.towerObj.tag == "Turret") {
                Image attackRadius = GameObject.Find("/NodeUI/Canvas/RadiusCanvas/AttackRadius").GetComponent<Image>();
                attackRadius.sprite = towerRadius;
            }

            if (selectedNode.GetIsUpgraded()) { 
                Button upgradeButton = GameObject.Find("/NodeUI/Canvas/Buttons/Upgrade").GetComponent<Button>();
                upgradeButton.GetComponent<Image>().sprite = upgradeDisable;
            } else {
                Button upgradeButton = GameObject.Find("/NodeUI/Canvas/Buttons/Upgrade").GetComponent<Button>();
                upgradeButton.GetComponent<Image>().sprite = upgradeDefault;
            }
        }
    }

    public void Hide()
    {
        ui.SetActive (false);
    }
}

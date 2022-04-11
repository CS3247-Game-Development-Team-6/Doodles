using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public GameObject ui;
    public Node target;
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

    private void FixedUpdate() {
        // Right clicking away when NodeUI is on now deactivates the NodeUI
        if (Input.GetMouseButtonDown(1) && ui.activeSelf) {
            ui.SetActive (false);
        }
    }

    public void SetTarget(Node _target)
    {
        /* Node UI location is dynamically set, with some hardcoded values here. */
        target = _target;
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

        transform.position = target.GetTowerBuildPosition() + new Vector3(xOffset, 0, zOffset);

        if (target.GetIsTowerBuilt())
        {
            ui.SetActive(true);


            if (target.tower.GetComponent<Turret>().bulletPrefab.tag == "Fire")
            {
                Button fireButton = GameObject.Find("/NodeUI/Canvas/Elements/Fire").GetComponent<Button>();
                Button iceButton = GameObject.Find("/NodeUI/Canvas/Elements/Ice").GetComponent<Button>();
                Button waterButton = GameObject.Find("/NodeUI/Canvas/Elements/Water").GetComponent<Button>();

                fireButton.GetComponent<Image>().sprite = fireActive;
                iceButton.GetComponent<Image>().sprite = iceDisable;
                waterButton.GetComponent<Image>().sprite = waterDisable;

            }
            else if (target.tower.GetComponent<Turret>().bulletPrefab.tag == "Ice")
            {
                Button fireButton = GameObject.Find("/NodeUI/Canvas/Elements/Fire").GetComponent<Button>();
                Button iceButton = GameObject.Find("/NodeUI/Canvas/Elements/Ice").GetComponent<Button>();
                Button waterButton = GameObject.Find("/NodeUI/Canvas/Elements/Water").GetComponent<Button>();

                fireButton.GetComponent<Image>().sprite = fireDisable;
                iceButton.GetComponent<Image>().sprite = iceActive;
                waterButton.GetComponent<Image>().sprite = waterDisable;
            }
            else if (target.tower.GetComponent<Turret>().bulletPrefab.tag == "Water")
            {
                Button fireButton = GameObject.Find("/NodeUI/Canvas/Elements/Fire").GetComponent<Button>();
                Button iceButton = GameObject.Find("/NodeUI/Canvas/Elements/Ice").GetComponent<Button>();
                Button waterButton = GameObject.Find("/NodeUI/Canvas/Elements/Water").GetComponent<Button>();

                fireButton.GetComponent<Image>().sprite = fireDisable;
                iceButton.GetComponent<Image>().sprite = iceDisable;
                waterButton.GetComponent<Image>().sprite = waterActive;
            }
            else 
            {
                Button fireButton = GameObject.Find("/NodeUI/Canvas/Elements/Fire").GetComponent<Button>();
                Button iceButton = GameObject.Find("/NodeUI/Canvas/Elements/Ice").GetComponent<Button>();
                Button waterButton = GameObject.Find("/NodeUI/Canvas/Elements/Water").GetComponent<Button>();

                fireButton.GetComponent<Image>().sprite = fireDefault;
                iceButton.GetComponent<Image>().sprite = iceDefault;
                waterButton.GetComponent<Image>().sprite = waterDefault;
            }

            if (target.tower.tag == "Missile")
            {
                Image attackRadius = GameObject.Find("/NodeUI/Canvas/RadiusCanvas/AttackRadius").GetComponent<Image>();
                attackRadius.sprite = missleRadius;
            }
            else if (target.tower.tag == "Turret")
            {
                Image attackRadius = GameObject.Find("/NodeUI/Canvas/RadiusCanvas/AttackRadius").GetComponent<Image>();
                attackRadius.sprite = towerRadius;
            }

            if (target.GetIsUpgraded())
            { 
                Button upgradeButton = GameObject.Find("/NodeUI/Canvas/Buttons/Upgrade").GetComponent<Button>();
                upgradeButton.GetComponent<Image>().sprite = upgradeDisable;
            }
            else
            {
                // if current ink is less than the tower's required upgrade ink, we mark the upgrade icon as disabled
                // we need reference to the player's ink level
                // we need reference to the tile's tower built -> then retrieve the amount of ink needed for upgrade

                if (playerGO.GetComponent<Player>().hasEnoughInk(target.tower.GetComponent<Turret>().upgradeCost))
                {
                    Button upgradeButton = GameObject.Find("/NodeUI/Canvas/Buttons/Upgrade").GetComponent<Button>();
                    upgradeButton.GetComponent<Image>().sprite = upgradeDefault;
                }
                else
                {
                    Button upgradeButton = GameObject.Find("/NodeUI/Canvas/Buttons/Upgrade").GetComponent<Button>();
                    upgradeButton.GetComponent<Image>().sprite = upgradeDisable;
                }

            }
        }
    }

    public void Update()
    {
        if (ui.activeInHierarchy)
        {
            if (target.GetIsUpgraded())
            {
                Button upgradeButton = GameObject.Find("/NodeUI/Canvas/Buttons/Upgrade").GetComponent<Button>();
                upgradeButton.GetComponent<Image>().sprite = upgradeDisable;
            }
            else
            {
                // if current ink is less than the tower's required upgrade ink, we mark the upgrade icon as disabled
                // we need reference to the player's ink level
                // we need reference to the tile's tower built -> then retrieve the amount of ink needed for upgrade

                if (playerGO.GetComponent<Player>().hasEnoughInk(target.tower.GetComponent<Turret>().upgradeCost))
                {
                    Button upgradeButton = GameObject.Find("/NodeUI/Canvas/Buttons/Upgrade").GetComponent<Button>();
                    upgradeButton.GetComponent<Image>().sprite = upgradeDefault;
                }
                else
                {
                    Button upgradeButton = GameObject.Find("/NodeUI/Canvas/Buttons/Upgrade").GetComponent<Button>();
                    upgradeButton.GetComponent<Image>().sprite = upgradeDisable;
                }

            }
        }
    }

    public void Hide()
    {
        ui.SetActive (false);
    }
}

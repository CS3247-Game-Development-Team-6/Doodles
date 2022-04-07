using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public GameObject ui;
    public Node target;

    public float zOffset;
    public float xOffsetMultiplier;
    private float xOffset;
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


    public void SetTarget(Node _target)
    {

        target = _target;
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(_target.transform.position);
        Debug.Log(screenPoint);
        xOffset = (0.5f - screenPoint.x / Camera.main.pixelWidth) * xOffsetMultiplier;
        Debug.Log(xOffset);

        transform.position = target.GetTowerBuildPosition() + new Vector3(xOffset, 0, zOffset);

        if (target.GetIsTowerBuilt())
        {
/*            Debug.Log("We are currently displaying it here: " + transform.position);*/
            ui.SetActive(true);


            if (target.tower.GetComponent<Turret>().bulletPrefab.tag == "Fire")
            {
                Debug.Log("Current selected bullet is fire");
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

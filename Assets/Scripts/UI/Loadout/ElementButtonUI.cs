using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class ElementButtonUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

    private TowerDescriptionUI descUI;
    [SerializeField] private bool isUpgrade;
    [SerializeField] private ElementType type;
    private TowerInfo previousTowerInfo;
    private TowerInfo myTowerInfo;
    private Image image;
    private CanvasGroup canvasGroup;

    [TextArea(2,4), SerializeField] private string templateElementTowerInfo = "";

    private void Start() {
        descUI = GetComponentInParent<TowerDescriptionUI>();
        image = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        descUI.SetUpgradesElements += SetMyValue;
    }

    public void SetMyValue(object sender, EventArgs e) {
        if (!descUI) {
            DisableButton();
            return;
        }
        previousTowerInfo = descUI.towerInfo;
        if (!previousTowerInfo) {
            DisableButton();
            return;
        }

        if (isUpgrade) {
            myTowerInfo = descUI.towerInfo.nextUpgrade;
            // descUI.SetInfo(descUI.towerInfo.nextUpgrade, false);
        } else {
            foreach (var elementTower in descUI.towerInfo.nextElements) {
                if (elementTower.element != type) continue;
                myTowerInfo = elementTower.tower;
                // descUI.SetInfo(elementTower.tower, false);
            }
        }

        if (myTowerInfo == null) {
            DisableButton();
        } else {
            EnableButton();
        }
    }

    public void DisableButton() {
        canvasGroup.alpha = 0.2f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void EnableButton() {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        // TODO: Remember to change the Sprite once updated
        if (!descUI) return;
        if (myTowerInfo == null) return;
        if (myTowerInfo.towerDesc.Length == 0) {
            myTowerInfo.towerDesc = templateElementTowerInfo;
        }
        descUI.SetInfo(myTowerInfo, false);
        /*
        previousTowerInfo = descUI.towerInfo;
        if (!previousTowerInfo) return;
        if (isUpgrade) {
            descUI.SetInfo(descUI.towerInfo.nextUpgrade, false);
        } else {
            foreach (var elementTower in descUI.towerInfo.nextElements) {
                if (elementTower.element != type) continue;
                descUI.SetInfo(elementTower.tower, false);
            }
        }
        */
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!descUI) return;
        descUI.SetInfo(previousTowerInfo);
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (!descUI) return;
        // For shop
    }
}

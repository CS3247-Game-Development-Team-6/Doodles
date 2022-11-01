using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ActionType {
    UPGRADE,
    FIX,
    DESTROY,
    ELEMENT
}

public class NodeButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    protected ActionType action;
    public Tower tower;
    public TextMeshProUGUI costUi;
    protected CanvasGroup canvasGroup;
    [TextArea(2,4), SerializeField] private string description = "";

    public void Init() {
        costUi = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void SetTower(Tower tower) {
        this.tower = tower;
    }

    public virtual void OnPointerEnter(PointerEventData e) { }
    public virtual void OnPointerExit(PointerEventData e) { }
    public virtual void OnPointerDown(PointerEventData e) { }
}



using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    private TabGroup tabGroup;
    public GameObject tab;

    public TextMeshProUGUI text { get; private set; }
    public Image background { get; private set; }

    private void Start() {
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        tabGroup = GetComponentInParent<TabGroup>();
        tabGroup.Subscribe(this);
    }

    public void OnPointerEnter(PointerEventData data) {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData data) {
        tabGroup.OnTabExit(this);
    }

    public void OnPointerDown(PointerEventData data) {
        tabGroup.OnTabSelected(this);
    }
}

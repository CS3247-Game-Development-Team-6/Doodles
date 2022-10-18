using UnityEngine;

public class TooltipSystem : MonoBehaviour {
    public static TooltipSystem instance { get; private set; }

    public Tooltip tooltip;
    

    private void Awake() {
        instance = this;
        instance.tooltip.gameObject.SetActive(true); // added this line to hide the default tooltip from the Editor
        instance.tooltip.SetVisibility(false);
    }

    public static void Show(string content, string header="") {
        instance.tooltip.SetText(content, header);
        instance.tooltip.SetVisibility(true);
    }

    public static void Hide() {
        if (!instance) return;
        instance.tooltip.SetVisibility(false);
    }



}


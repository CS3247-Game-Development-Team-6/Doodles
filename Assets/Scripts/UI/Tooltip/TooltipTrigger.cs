using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{    
    [SerializeField] protected string header;
    
    [Multiline]
    [SerializeField] protected string content;
    
    public virtual void OnPointerEnter(PointerEventData eventData) {
        TooltipSystem.Show(content, header);
    }

    public void OnPointerExit(PointerEventData eventData) {
        TooltipSystem.Hide();
    }
    
    public void OnPointerDown(PointerEventData eventData) {
        // in-case we click the button, we can hide the tooltip again. For example if we click a button.
        TooltipSystem.Hide();
    }
}

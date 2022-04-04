using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{    
    [SerializeField] private string header;
    
    [Multiline]
    [SerializeField] private string content;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Show(content, header);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        // in-case we click the button, we can hide the tooltip again. For example if we click a button.
        TooltipSystem.Hide();
    }
}

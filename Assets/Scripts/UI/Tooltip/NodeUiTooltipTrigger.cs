using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeUiTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{    
    [SerializeField] private string header;

    [SerializeField] private string cannonCost;

    [SerializeField] private string missileCost;

    private string costText = "Ink cost: ";
    
    [Multiline]
    [SerializeField] private string content;
    
    private static Node currentNode;

    public void SetNode(Node node) {
        Debug.Log("SET NODE!!");
        currentNode = node;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentNode.tower.tag == "Missile") {
            TooltipSystem.Show(costText + missileCost + "\n" + content, header);
        } else if (currentNode.tower.tag == "Turret") {
            TooltipSystem.Show(costText + cannonCost + "\n" + content, header);
        } else {
            TooltipSystem.Show(content, header);
        }
        
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


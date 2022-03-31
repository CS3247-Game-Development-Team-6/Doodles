using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;

    public Tooltip tooltip;
    

    private void Awake()
    {
        current = this;
        current.tooltip.gameObject.SetActive(true); // added this line to hide the default tooltip from the Editor
        current.tooltip.SetVisibility(false);
    }

    public static void Show(string content, string header="")
    {
        current.tooltip.SetText(content, header);
        current.tooltip.SetVisibility(true);
    }

    public static void Hide()
    {
        current.tooltip.SetVisibility(false);
    }



}


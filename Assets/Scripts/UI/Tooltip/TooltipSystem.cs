using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem _current;

    public Tooltip tooltip;
    

    private void Awake()
    {
        _current = this;
        _current.tooltip.gameObject.SetActive(true); // added this line to hide the default tooltip from the Editor
        _current.tooltip.SetVisibility(false);
    }

    public static void Show(string content, string header="")
    {
        _current.tooltip.SetText(content, header);
        _current.tooltip.SetVisibility(true);
    }

    public static void Hide()
    {
        _current.tooltip.SetVisibility(false);
    }



}


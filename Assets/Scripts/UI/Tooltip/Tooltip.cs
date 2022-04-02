using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI headerField;
    [SerializeField] private TextMeshProUGUI contentField;
    [SerializeField] private LayoutElement layoutElement;

    [SerializeField] private float secondsToWaitBeforeShowing = 0.4f;
    [SerializeField] private float secondsToFadeIn = 1f;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private bool tooltipIsOn;

    // This script is based off Game Dev Guide's tooltip video: https://youtu.be/HXFoUGw7eKk

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetText(string content, string header="")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;

        layoutElement.enabled = Math.Max(headerField.preferredWidth, contentField.preferredWidth) 
                                >= layoutElement.preferredWidth;

    }

    public void SetVisibility(bool onOrOff)
    {
        tooltipIsOn = onOrOff;

        if (tooltipIsOn) StartCoroutine(nameof(FadeIn));
        if (!tooltipIsOn)
        {
            StopCoroutine(nameof(FadeIn));
            canvasGroup.alpha = 0;
        }
    }

    private IEnumerator FadeIn()
    {
        for (float a = 0.00f; a < secondsToWaitBeforeShowing + secondsToFadeIn; a += 0.03f)
        {
            if (!tooltipIsOn) yield break;
            
            if (a >= secondsToWaitBeforeShowing) canvasGroup.alpha = a - secondsToWaitBeforeShowing;
            else MoveTooltip(); // move the tooltip every iteration to the current mouse position
            
            yield return new WaitForSeconds(0.03f);
        }
    }

    private void MoveTooltip()
    {
        Vector2 mousePosition = Input.mousePosition;

        int scWidth = Screen.width;
        int scHeight = Screen.height;
        
        float pivotX = mousePosition.x / scWidth;
        float pivotY = mousePosition.y / scHeight;

        rectTransform.pivot = new Vector2(pivotX, pivotY);

        Vector3 tooltipPosition = mousePosition;

        tooltipPosition.x = mousePosition.x < (float) scWidth / 2
            ? tooltipPosition.x + (float) scWidth / 25
            : tooltipPosition.x - (float) scWidth / 25;

        transform.position = tooltipPosition;
    }
}

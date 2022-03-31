using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ADBannerView = UnityEngine.iOS.ADBannerView;

public class Tooltip : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI headerField;
    [SerializeField] private TextMeshProUGUI contentField;
    [SerializeField] private LayoutElement layoutElement;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private bool tooltipIsOn = false;
    private Vector3 centerPos;

    // This script is based off Game Dev Guide's tooltip video: https://youtu.be/HXFoUGw7eKk

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (Camera.main != null)
        {
            centerPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        }
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

        layoutElement.enabled = Math.Max(headerField.preferredWidth, contentField.preferredWidth) >= layoutElement.preferredWidth;

    }

    public void SetVisibility(bool onOrOff)
    {
        tooltipIsOn = onOrOff;

        if (tooltipIsOn) StartCoroutine(nameof(FadeIn));
        if (!tooltipIsOn) canvasGroup.alpha = 0;
    }

    IEnumerator FadeIn()
    {
        for (float f = 0.00f; f < 1.4; f+=0.05f)
        {
            if (!tooltipIsOn) yield break;
            
            if (f >= 0.4f) canvasGroup.alpha = f - 0.4f;
            
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;

        int scWidth = Screen.width;
        int scHeigth = Screen.height;


        float pivotX = mousePosition.x / scWidth;
        float pivotY = mousePosition.y / scHeigth;

        rectTransform.pivot = new Vector2(pivotX, pivotY);

        Vector3 tooltipPosition = mousePosition;

        if (mousePosition.x < scWidth / 2)
        {
            tooltipPosition.x += scWidth / 25;
        }
        else
        {
            tooltipPosition.x -= scWidth / 25;
        }

        transform.position = tooltipPosition;
    }
}

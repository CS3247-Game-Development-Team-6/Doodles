using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorUI : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Image slider;
    [SerializeField] private Color criticalColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color criticalTextColor;
    [SerializeField] private Color normalTextColor;
    [SerializeField] private float criticalLevel;
    public int rawValue;
    public int maxValue;

    private void Update() {
        text.text = rawValue.ToString();
        slider.fillAmount = maxValue > 0 ? Mathf.Min(1f, (float)rawValue / maxValue) : 0;
        slider.color = (slider.fillAmount < criticalLevel) ? criticalColor : normalColor;
        text.color = (slider.fillAmount < criticalLevel) ? criticalTextColor : normalTextColor;
    }
}

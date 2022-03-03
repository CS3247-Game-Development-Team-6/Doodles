using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public const float maxInk = 100f;
    public Image healthSlider;
    public float growthRate;
    private float ink = 60f;

    public bool hasEnoughInk(float cost) {
        return ink >= cost;
    }

    public void ChangeInkAmount(float deltaAmount) {
        ink = Mathf.Min(ink + deltaAmount, maxInk);
        ink = Mathf.Max(ink, 0.0f);
        healthSlider.fillAmount = ink / maxInk;
    }

    private void Update() {
        if (ink < maxInk) {
            ChangeInkAmount(growthRate);
        }
    }
}

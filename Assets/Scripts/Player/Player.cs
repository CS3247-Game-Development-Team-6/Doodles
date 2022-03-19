using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public const float maxInk = 100f;
    public Image healthSlider;
    public float growthRate;
    [Range(0, 1)]
    public float startingAmount = 0.6f;
    private float ink;

    private void Start() {
        ink = startingAmount * maxInk;
    }

    public bool hasEnoughInk(float cost) {
        return ink >= cost;
    }

    public void ChangeInkAmount(float deltaAmount) {
        // at most maxInk
        ink = Mathf.Min(ink + deltaAmount, maxInk);

        // at least 0
        ink = Mathf.Max(ink, 0.0f);

        // update visual
        healthSlider.fillAmount = ink / maxInk;
    }

    // from killing enemy
    public void AddInk(float inkAmount) {
        ink = ink + inkAmount;
    }

    private void Update() {
        if (ink < maxInk) {
            ChangeInkAmount(growthRate);
        }
    }
}

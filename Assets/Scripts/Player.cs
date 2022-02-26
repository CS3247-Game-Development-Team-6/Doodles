using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public const float maxInk = 100f;
    public Image healthSlider;
    private float ink = 20f;

    public void ChangeInkAmount(float deltaAmount) {
        ink = Mathf.Min(ink + deltaAmount, maxInk);
        healthSlider.fillAmount = ink / maxInk;
    }

    private void Update() {
        if (ink < maxInk) {
            ChangeInkAmount(0.01f);
        }
    }
}

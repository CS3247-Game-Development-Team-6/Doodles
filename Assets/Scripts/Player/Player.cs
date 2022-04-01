using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public const float maxInk = 100f;
    public float growthRate;
    [Range(0, 1)] public float startingAmount = 0.6f;
    private float ink;
    [SerializeField] private PlayerMovement movement;

    public PlayerMovement Movement => movement;
    [SerializeField] private IndicatorUI playerInkIndicator;

    private void Start() {

        ink = startingAmount * maxInk;
        playerInkIndicator.maxValue = (int)maxInk;
        playerInkIndicator.rawValue = (int)ink;
        //Debug.Log(playerInkIndicator.maxValue);
        //Debug.Log(playerInkIndicator.rawValue);
    }

    public bool hasEnoughInk(float cost) {
        return ink >= cost;
    }

    public void ChangeInkAmount(float deltaAmount) {
        // at most maxInk
        ink = Mathf.Min(ink + deltaAmount, maxInk);

        // at least 0
        ink = Mathf.Max(ink, 0.0f);
        playerInkIndicator.rawValue = (int)ink;

        // update visual
    }

    // from killing enemy
    public void AddInk(float inkAmount) {
        ink = ink + inkAmount;
        playerInkIndicator.rawValue = (int)ink;
    }

    private void Update() {
        if (ink < maxInk) {
            ChangeInkAmount(growthRate);
        }
    }
}

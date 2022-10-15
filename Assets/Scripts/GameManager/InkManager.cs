using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InkManager : MonoBehaviour {

    public static InkManager instance { get; private set; }
    private float maxInk = 400f;
    private float growthRate;
    private float ink;
    private MapInfo mapInfo;

    // Deprecating
    [SerializeField] private IndicatorUI playerInkIndicator;

    public float globalInkGainMultiplier { get; private set; } = 1.0f;
    public float InkFraction => maxInk == 0 ? 0 : ink / maxInk;
    public string InkString => $"{ink} / {maxInk}";

    private void Start() {
        if (instance != null) {
            Debug.Log("InkManager should be a singleton! Only 1 should exist in a scene.");
            return;
        }
        instance = this;

        if (FindObjectOfType<Map>() != null) { 
            mapInfo = FindObjectOfType<Map>().MapInfo;
            if (mapInfo != null) {
                growthRate = mapInfo.inkRegenRate;
                maxInk = mapInfo.totalInk;
            }
        }


        ink = mapInfo.startingInkFraction * maxInk;
        playerInkIndicator.maxValue = (int)maxInk;
        playerInkIndicator.rawValue = (int)ink;
    }

    // Only applied on positive ink increments.
    public void SetInkGainMultiplier(float mult) {
        Debug.Log($"Set to {mult}");
        globalInkGainMultiplier = mult;
    }

    public bool hasEnoughInk(float cost) {
        return ink >= cost;
    }

    public void ChangeInkAmount(float deltaAmount) {
        // at most maxInk
        ink = Mathf.Min(ink + deltaAmount * (deltaAmount > 0 ? globalInkGainMultiplier : 1), maxInk);

        // at least 0
        ink = Mathf.Max(ink, 0.0f);
        playerInkIndicator.rawValue = (int)ink;

        // update visual
    }

    private void Update() {
        if (ink < maxInk && growthRate != 0) {
            ChangeInkAmount(growthRate);
        }
    }
}

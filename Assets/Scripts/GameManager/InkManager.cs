using UnityEngine;

public class InkManager : MonoBehaviour {

    public static InkManager instance { get; private set; }
    public float maxInk = 400f;
    public float growthRate;
    [Range(0, 1)] public float startingAmount = 0.6f;
    private float ink;
    [SerializeField] private PlayerMovement movement;
    private MapInfo mapInfo;

    public PlayerMovement Movement => movement;
    [SerializeField] private IndicatorUI playerInkIndicator;

    private void Start() {
        if (instance != null) {
            Debug.Log("InkManager should be a singleton! Only 1 should exist in a scene.");
            return;
        }
        instance = this;

        if (FindObjectOfType<Map>() != null) { 
            mapInfo = FindObjectOfType<Map>().MapInfo;
            if (mapInfo != null) {
                startingAmount = mapInfo.startingInkFraction;
                growthRate = mapInfo.inkRegenRate;
            }
        }


        ink = startingAmount * maxInk;
        playerInkIndicator.maxValue = (int)maxInk;
        playerInkIndicator.rawValue = (int)ink;
    }

    public bool hasEnoughInk(float cost) {
        return ink >= cost;
    }

    public void ChangeInkAmount(float deltaAmount) {
        // at most maxInk
        deltaAmount = deltaAmount * (SpellManager.instance.InkFactor);
        ink = Mathf.Min(ink + deltaAmount, maxInk);
        // at least 0
        ink = Mathf.Max(ink, 0.0f);
        playerInkIndicator.rawValue = (int)ink;

        // update visual
    }

    private void Update() {
        if (ink < maxInk) {
            ChangeInkAmount(growthRate);
        }
    }
}

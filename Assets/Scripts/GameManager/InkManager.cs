using UnityEngine;

public class InkManager : MonoBehaviour {

    public static InkManager instance { get; private set; }
    private float maxInk = 400f;
    private float growthRate;
    private float ink;
    [SerializeField] private PlayerMovement movement;
    private MapInfo mapInfo;

    public PlayerMovement Movement => movement;
    // Deprecating
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
                growthRate = mapInfo.inkRegenRate;
                maxInk = mapInfo.totalInk;
            }
        }


        ink = mapInfo.startingInkFraction * maxInk;
        playerInkIndicator.maxValue = (int)maxInk;
        playerInkIndicator.rawValue = (int)ink;
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

    private void Update() {
        if (ink < maxInk && growthRate != 0) {
            ChangeInkAmount(growthRate);
        }
    }
}

using System.IO;
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

        // If no load found, set default values
        if (!Load()) {
            ink = startingAmount * maxInk;
            playerInkIndicator.maxValue = (int)maxInk;
            playerInkIndicator.rawValue = (int)ink;
        }
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

    public void SetInkAmount(float amount) {
        // at most maxInk
        ink = Mathf.Min(amount, maxInk);

        // at least 0
        ink = Mathf.Max(ink, 0.0f);
        playerInkIndicator.rawValue = (int)ink;

        // update visual
    }

    public void Save() {
        InkData data = new InkData {
            ink = this.ink,
            maxInk = this.maxInk,
        };

        string json = JsonUtility.ToJson(data);
        string path = Application.dataPath + "/ink.json";
        File.WriteAllText(path, json);
        Debug.Log($"Saving: {json} at {path}");

    }

    public bool Load() {
        string path = Application.dataPath + "/ink.json";
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            InkData data = JsonUtility.FromJson<InkData>(json);
            maxInk = data.maxInk;
            SetInkAmount(data.ink);
            Debug.Log($"ink {data.ink}; max {maxInk};");
            return true;
        } else {
            Debug.Log("no save found.");
            return false;
        }
    }

    private void Update() {
        if (ink < maxInk && growthRate > 0) {
            ChangeInkAmount(growthRate);
        }
    }

    private class InkData {
        public float maxInk;
        public float ink;
    }
}

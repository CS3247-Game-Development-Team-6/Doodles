using System.Collections.Generic;
using UnityEngine;

public class PlayerXP : MonoBehaviour {
    [SerializeField] private float xpPerChunk; 
    [SerializeField] private List<float> xpToUnlockSpellLevel;

    private float xp;

    private void Start() {
        xp = 0;
    }

    public void IncreaseByXPPerChunk() {
        xp += xpPerChunk;
    }
}

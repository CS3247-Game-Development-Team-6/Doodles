using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour {
    public Chunk chunk;
    public int maxHealth = 500;

    public int hp { get;  private set; }
    public float HpFract => (float)hp / maxHealth;

    private void Awake() {
        hp = maxHealth;
    }

    public void TakeDmg(int amount) {
        hp -= amount;
    }

    public bool isHpLow() {
        return HpFract < 0.25f;
    }
}

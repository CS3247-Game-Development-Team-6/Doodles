using UnityEngine;

public enum ElementEffectType {
    FIRE, ICE, WATER, COMBINED, NONE
}

[CreateAssetMenu(menuName = "Status Effect")]
public class ElementEffectInfo : ScriptableObject {
    public string Name; // to be removed
    public float DOTAmount;
    public float TickSpeed;
    public float SlowAmount;        // Between 0 to 1
    public int AtkDecreAmount;
    public int DefDecreAmount;
    public float Lifetime;
    public ElementEffectType Element;

    public GameObject EffectParticles;
}

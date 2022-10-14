using UnityEngine;

public enum ElementType {
    FIRE, ICE, WATER
}

[CreateAssetMenu(fileName = "NewElement", menuName = "Element Info")]
public class ElementInfo : ScriptableObject {
    public ElementType type;
    public ElementEffectInfo effect;
    public ElementType weakness;
}

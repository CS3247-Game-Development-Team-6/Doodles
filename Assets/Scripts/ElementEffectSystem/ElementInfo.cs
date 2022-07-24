using UnityEngine;

public enum ElementType {
    FIRE, WATER, ICE
}

[CreateAssetMenu(fileName = "NewElement", menuName = "Element Info")]
public class ElementInfo : ScriptableObject {
    public ElementType type;
    public ElementEffectInfo effect;
}

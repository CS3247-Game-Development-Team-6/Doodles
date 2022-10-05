using UnityEngine;

public class Spell : MonoBehaviour {

    [SerializeField] public string effectName;

    [SerializeField] public float cost;
    [SerializeField] public float duration;

    public virtual void Activate() {
        Debug.Log($"Effect {effectName} run!");
    }
    public virtual void Deactivate() { }

}

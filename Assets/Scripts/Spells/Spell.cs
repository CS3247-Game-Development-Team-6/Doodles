using UnityEngine;

public class GlobalEffect : MonoBehaviour {

    [SerializeField] public string effectName;

    [SerializeField] public float cost;
    [SerializeField] public float duration;

    public virtual void Activate() {
        Debug.Log($"Effect {effectName} run!");
    }
    public virtual void Deactivate() { }

}

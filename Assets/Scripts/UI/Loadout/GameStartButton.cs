using UnityEngine;

public class GameStartButton : MonoBehaviour {
    private Loadout loadout;
    private void Start() {
        loadout = FindObjectOfType<Loadout>();
        if (!loadout) {
            Debug.LogError("Loadout not found in hierarchy, level will not load.");
        }
    }

    public void OnClick() {
        if (!loadout) {
            Debug.LogError("Loadout not found in hierarchy, level will not load.");
            return;
        }
        loadout.StartGame();
    }
}

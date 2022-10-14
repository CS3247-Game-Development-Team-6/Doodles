using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Loadout : MonoBehaviour {
    public TextMeshProUGUI title;
    public GameObject infoMenu;
    public GameObject shopBar;
    public FocusedInfoUI focusedInfoBox;
    public Button goToScene;
    public LoadingUI loadingScreen;
    public static MapInfo mapToLoad;

    private void Start() {
        if (!mapToLoad) {
            Debug.Log("No map to load");
            return;
        }

        title.text = mapToLoad.levelName;
    }

    public void StartGame() {
        if (!mapToLoad) {
            Debug.Log("No map to load");
            return;
        }

        focusedInfoBox.MoveToHUDPos();
        Destroy(title);
        Destroy(infoMenu);
        Destroy(goToScene);
        loadingScreen.gameObject.SetActive(true);
        // loadingScreen.AddSceneToUnload(SceneManager.GetActiveScene().name);
        loadingScreen.AddSceneToLoad(mapToLoad.gameSceneName);
        loadingScreen.StartLoad();
        Destroy(this);
    }
}

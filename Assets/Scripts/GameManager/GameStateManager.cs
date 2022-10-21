using UnityEngine;

public class GameStateManager : MonoBehaviour {
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject winUI;

    private static bool isGameEnded;
    private Map map;

    void Start() {
        isGameEnded = false;
        map = FindObjectOfType<Map>();
    }

    void Update() {
        if (isGameEnded)
            return;

        if (map && map.currentChunk && map.currentChunk.Base && map.currentChunk.Base.hp <= 0) {
            EndGame();
        }
    }

    public void EndGame() {
        isGameEnded = true;
        gameOverUI.SetActive(true);
    }

    public void WinGame() {
        isGameEnded = true;
        winUI.SetActive(true);
    }

    // for other game scripts to check if the game is ended
    public static bool getIsGameEnded() {
        return isGameEnded;
    }

    public static GameObject[] GetDontDestroyOnLoadObjects() {
        GameObject temp = null;
        try {
            temp = new GameObject();
            Object.DontDestroyOnLoad(temp);
            UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
            Object.DestroyImmediate(temp);
            temp = null;

            return dontDestroyOnLoad.GetRootGameObjects();
        } finally {
            if (temp != null)
                Object.DestroyImmediate(temp);
        }
    }

}

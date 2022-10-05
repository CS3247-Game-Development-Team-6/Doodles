using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject winUI;
    public string nextSceneName;

    private static bool isGameEnded;
    private Map map;

    void Start() {
        isGameEnded = false;
        map = FindObjectOfType<Map>();

        GameObject[] ddols = GetDontDestroyOnLoadObjects();
        for (int i = 0; i < ddols.Length; i++) {
            ddols[i].transform.Find("LoadingBar").gameObject.SetActive(false);
        }

        Load();
    }

    public void Save() {
        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        GameData data = new GameData {
            health = player.healthAmount,
            maxHealth = player.maxHealth,
            playerPos = player.transform.position
        };

        string json = JsonUtility.ToJson(data);
        string path = Application.dataPath + "/player.json";
        File.WriteAllText(path, json);
        Debug.Log($"Saving: {json} at {path}");
    }

    public void Load() {
        string path = Application.dataPath + "/player.json";
        if (!File.Exists(path)) {
            Debug.LogWarning("No save found");
            return;
        }

        string json = File.ReadAllText(path);
        GameData data = JsonUtility.FromJson<GameData>(json);
        Debug.Log("player pos: " + data.playerPos);
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
        GameObject[] ddols = GetDontDestroyOnLoadObjects();
        for (int i = 0; i < ddols.Length; i++) {
            ddols[i].SetActive(false);
        }

        isGameEnded = true;

        winUI.SetActive(true);
    }

    public void GotoNextScene() {
        GameObject[] ddols = GetDontDestroyOnLoadObjects();
        for (int i = 0; i < ddols.Length; i++) {
            Destroy(ddols[i]);
        }
        SceneManager.LoadScene(nextSceneName);
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

    private class GameData {
        public float health;
        public float maxHealth;
        public Vector3 playerPos;

    }
}

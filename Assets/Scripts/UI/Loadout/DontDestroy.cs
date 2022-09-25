using UnityEngine;


/**
 * ref: https://www.youtube.com/watch?v=HXaFLm3gQws
 */
public class DontDestroy : MonoBehaviour {
    public static DontDestroy instance;

    // Start is called before the first frame update
    void Start() {
        // only one object can be non-destroy :-)
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

}

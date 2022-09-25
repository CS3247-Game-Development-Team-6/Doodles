using UnityEngine;

/** Barrier for each map Chunk. If isTrigger, then */
[RequireComponent(typeof(Collider))]
public class Barrier : MonoBehaviour {
    private Collider player;

    private void Start() {
        player = FindObjectOfType<PlayerMovement>().GetComponent<Collider>();
    }

    private void OnTriggerExit(Collider other) {
        if (other == player) {
            // Debug.Log("EXIT " + name);
            GetComponent<Collider>().isTrigger = false;
        }
    }
}

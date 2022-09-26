using System;
using UnityEngine;

/** Barrier for each map Chunk. If isTrigger, then */
[RequireComponent(typeof(Collider))]
public class Barrier : MonoBehaviour {
    private Chunk chunk;
    private Collider player;
    public event EventHandler CrossBarrier;
    public event EventHandler CloseBarrier;
    public Chunk NextChunk => chunk == null || chunk.nextChunk == null ? null : chunk.nextChunk;
    public Chunk Chunk => chunk;

    private void Start() {
        chunk = GetComponentInParent<Chunk>();
        player = FindObjectOfType<PlayerMovement>().GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other == player) {
            Debug.Log("ENTER " + name);
            // Give details of nextChunk
            CrossBarrier?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other == player) {
            if (!chunk.ContainsPosition(player.transform.position)) {
                Debug.Log("EXIT " + name);
                GetComponent<Collider>().isTrigger = false;
                // Give details of currChunk to close
                CloseBarrier?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

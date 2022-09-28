using System;
using UnityEngine;

/** Barrier for each map Chunk. If isTrigger, then */
[RequireComponent(typeof(Collider))]
public class Barrier : MonoBehaviour {
    private DIR dir;
    private Chunk chunk;
    private Collider player;
    private ParticleSystem cloud;
    public event EventHandler CrossBarrier;
    public event EventHandler CloseBarrier;
    public Chunk NextChunk => chunk == null || chunk.nextChunk == null ? null : chunk.nextChunk;
    public Chunk Chunk => chunk;

    private bool isInitialized = false;
    [SerializeField] public float lifetimemult;

    public void Init(DIR dir) {
        this.dir = dir;
    }

    private void Start() {
        chunk = GetComponentInParent<Chunk>();
        player = FindObjectOfType<PlayerMovement>().GetComponent<Collider>();
        cloud = transform.Find("Barrier Cloud").GetComponent<ParticleSystem>();
    }

    private void Update() {
        if (!isInitialized && cloud != null) {
            if (dir == DIR.RIGHT || dir == DIR.LEFT) {
                cloud.transform.Rotate(0, -90, 0);
                cloud.transform.position += Vector3.right * chunk.cellSize * chunk.gridSize.y / 2;
            }
            var main = cloud.main;
            main.startLifetimeMultiplier = lifetimemult * chunk.cellSize * Mathf.Max(chunk.gridSize.x, chunk.gridSize.y);
            isInitialized = true;
        }
        if (GetComponent<Collider>().isTrigger) { 
            cloud.Stop();
        } else if (cloud.isStopped) {
            cloud.Play();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other == player) {
            // Debug.Log("ENTER " + name);
            // Give details of nextChunk
            CrossBarrier?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other == player) {
            if (!chunk.ContainsPosition(player.transform.position)) {
                // Debug.Log("EXIT " + name);
                GetComponent<Collider>().isTrigger = false;
                // Give details of currChunk to close
                CloseBarrier?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

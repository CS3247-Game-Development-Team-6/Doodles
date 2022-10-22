using UnityEngine;

public class MapInput : MonoBehaviour { 
 
    private Camera cam;
    private InkManager inkManager;
    private Map map;
    public PlayerMovement playerMovement;
    public ParticleSystem insufficientInkEffect;

    void Start() {
        cam = Camera.main;
        inkManager = InkManager.instance;
        map = FindObjectOfType<Map>();
    }

    void Update() {
        if (!map) {
            Debug.LogError("MapInput: No Map found to control");
            return;
        }

        // Right click is detected
        bool ignoreInput = !SpellManager.instance || SpellManager.instance.isCasting;
        if (!ignoreInput && Input.GetMouseButtonDown(1)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                Fog fog = hit.transform.gameObject.GetComponent<Fog>();
                Node node = hit.transform.gameObject.GetComponent<Node>();
                if (fog != null) {
                    // If fog is not in current chunk, ignore
                    if (!map.currentChunk.ContainsPosition(fog.transform.position)) {
                        return;
                    }
                    if (inkManager.hasEnoughInk(fog.cost)) {
                        inkManager.ChangeInkAmount(-fog.cost);
                        fog.ClearFog();
                    } else {
                        Instantiate(insufficientInkEffect, playerMovement.transform.position, Quaternion.identity);
                    }
                } else if (node != null) {
                    if (node.cell.isFog) {
                        return;
                    }
                    Vector3 mouseTowerCellPosition = hit.collider.gameObject.transform.position;
                    playerMovement.BuildTowerAttempt(mouseTowerCellPosition, hit.collider.gameObject);
                }
            }
        }
    }
}

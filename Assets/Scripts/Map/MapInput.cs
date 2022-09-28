using UnityEngine;

public class MapInput : MonoBehaviour { 
 
    private Camera cam;
    private InkManager inkManager;
    public PlayerMovement playerMovement;
    public ParticleSystem insufficientInkEffect;

    void Start() {
        cam = Camera.main;
        inkManager = InkManager.instance;
    }

    void Update() {
        // Right click is detected
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                Fog fog = hit.transform.gameObject.GetComponent<Fog>();
                Node node = hit.transform.gameObject.GetComponent<Node>();
                if (fog != null) {
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

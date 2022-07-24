using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInput : MonoBehaviour { 
 
    private Camera cam;
    public MapGenerator map;
    public TowerManager towerManager;
    public InkManager inkManager;
    public PlayerMovement playerMovement;
    public GameObject playerGO;
    public GameObject insufficientInkEffect;

    void Start() {
        cam = Camera.main;
        towerManager = TowerManager.instance;
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
                        Instantiate(insufficientInkEffect, playerGO.transform.position, Quaternion.identity);
                    }
                } else if (node != null) {
                    if (node.cell.isFog) {
                        return;
                    }
                    Vector3 mouseTowerCellPosition = hit.collider.gameObject.transform.position;
                    // TO REMOVE once towerManager is complete
                    playerMovement.BuildTowerAttempt(mouseTowerCellPosition, hit.collider.gameObject);
                }
            }
        }
    }
}

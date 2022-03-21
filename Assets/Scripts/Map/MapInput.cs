using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInput : MonoBehaviour { 
 
    private Camera cam;
    public MapGenerator map;
    public Player player;

    private Fog fogHighlighted = null;
    private Node nodeHighlighted = null;

    void Start() {
        cam = Camera.main;
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
                    if (player.hasEnoughInk(fog.cost)) {
                        player.ChangeInkAmount(-fog.cost);
                        fog.ClearFog();
                    }
                    else {
                        Debug.Log("Not enough ink!");
                    }
                } else if (node != null) {
                    Vector3 mouseTowerCellPosition = hit.point;
                    player.Movement.BuildTowerAttempt(mouseTowerCellPosition, hit.collider.gameObject);
                }
            }
        }
    }
}

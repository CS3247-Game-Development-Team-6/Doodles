using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInput : MonoBehaviour { 
 
    private Camera cam;
    public MapGenerator map;
    public Player player;

    void Start() {
        cam = Camera.main;
    }

    void Update() {
        // Right click is detected
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.gameObject.name.Contains("Fog")) {
                    Fog fog = hit.transform.gameObject.GetComponent<Fog>();
                    if (player.hasEnoughInk(fog.cost)) {
                        player.ChangeInkAmount(-fog.cost);
                        fog.ClearFog();
                    } else {
                        Debug.Log("Not enough ink!");
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInput : MonoBehaviour { 
 
    private Camera cam;
    public Map map;

    void Start() {
        cam = Camera.main;
    }

    void Update() {
        // Left click is detected
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.Log("left");
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.gameObject.name.Contains("Fog")) {
                    // Debug.Log("Clicked on " + hit.transform.gameObject.name);
                    hit.transform.gameObject.GetComponent<FogTarget>().ClearFog();
                }
            }
        }
    }
}

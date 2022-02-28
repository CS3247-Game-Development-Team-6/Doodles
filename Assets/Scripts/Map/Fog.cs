using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour {

    public Cell cell;
    public Renderer renderer;

    void Start() {
        renderer = GetComponent<Renderer>();
    }

    public void ClearFog() {
        cell.isFog = false;
    }

    private void OnMouseEnter() {
        // Fog turns red if hovered over with mouse
        renderer.material.color = Color.red;
    }

    private void OnMouseExit() {
        // Fog reverts its original color if mouse leaves
        renderer.material.color = new Color32(110, 109, 109, 255);
    }
}


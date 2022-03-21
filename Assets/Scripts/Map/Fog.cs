using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour {

    public Cell cell;
    public Material defaultMat;
    public float cost;

    public void ClearFog() {
        cell.isFog = false;
    }
    
    /*
    public void HighlightEnter() {
        renderer.material.color = Color.red;
    }
    
    public void HighlightExit() {
        // Fog reverts its original color if mouse leaves
        renderer.material.color = defaultMat.color;
    }
    */

    private void OnMouseEnter() {
        // Fog turns red if hovered over with mouse
        GetComponent<Renderer>().material.color = Color.red;
    }

    private void OnMouseExit() {
        // Fog reverts its original color if mouse leaves
        GetComponent<Renderer>().material.color = defaultMat.color;
    }
}


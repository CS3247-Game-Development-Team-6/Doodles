using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour {

    public Cell cell;
    public Material defaultMat;
    public float cost;
    [SerializeField] private Color tint;
    public Renderer objectToTint;

    public void ClearFog() {
        cell.isFog = false;
        gameObject.SetActive(cell.isFog);
        cell.tile.SetActive(!cell.isFog);
    }
    
    private void OnMouseEnter() {
        // Fog turns red if hovered over with mouse
        objectToTint.material.color = tint;
    }

    private void OnMouseExit() {
        // Fog reverts its original color if mouse leaves
        objectToTint.material.color = defaultMat.color;
    }
}


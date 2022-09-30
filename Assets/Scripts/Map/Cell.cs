using System;
using UnityEngine;

public enum CellType {
    // Can change name 
    NONE,
    STRAIGHTPATH,
    CURVEPATH,
    BASE,
    SPAWN,
}


/** To hold relevant gameObjects, attributes etc.
 */
public class Cell {
    // index = (row, col)
    private Vector2Int index;
    public GameObject tile;
    public GameObject fog;
    public CellType type;
    public bool isFog;
    public Vector3 rotation; 
    public Vector3 position;
    public int pathOrder;

    public Vector2Int Index => index;

    public Vector3 Position { get; }

    public Cell(Vector2Int index, Vector3 pos, CellType typeOfCell, bool isFoggy, Vector3 rot, int pathNum) {
        this.index = index;
        position = pos;
        type = typeOfCell;
        isFog = isFoggy;
        rotation = rot;
        pathOrder = pathNum;
    }
}
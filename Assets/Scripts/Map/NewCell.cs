using System;
using UnityEngine;

public enum CellTypeNew {
    // Can change name 
    NONE,
    STRAIGHTPATH,
    CURVEPATH,
    BASE,
    SPAWN,
}


/** To hold relevant gameObjects, attributes etc.
 */
public class NewCell {
    // index = (row, col)
    private Vector2Int index;
    public GameObject tile;
    public GameObject fog;
    public CellTypeNew type;
    public bool isFog;
    public Vector3 rotation; 
    public Vector3 position;

    public Vector2Int Index {
        get { return index; }
    }

    public Vector3 Position { get; }

    /*
    public Cell() {
        this.type = CellType.NONE;
    }
    
    
    public Cell(Vector2Int index, Vector3 position) {
        this.index = index;
        this.Position = position;
        this.type = CellType.NONE;
    }
    */

    public NewCell(Vector3 pos, CellTypeNew typeOfCell, bool isFoggy, Vector3 rot) {
        position = pos;
        type = typeOfCell;
        isFog = isFoggy;
        rotation = rot;
    }
}
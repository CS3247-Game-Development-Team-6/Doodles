using UnityEngine;

public enum CellType {
    // Can change name 
    WALK,
    BASE,
    NONE
}


/** To hold relevant gameObjects, attributes etc.
 */
public class Cell {
    // index = (row, col)
    private Vector2Int index;
    private Vector3 position;
    public GameObject tile;
    public GameObject fog;
    public CellType type;
    public bool isFog;

    public Vector2Int Index {
        get { return index; }
    }

    public Vector3 Position {
        get { return position; }
    }
    
    public Cell(Vector2Int index, Vector3 position, CellType type, bool isFog) {
        this.index = index;
        this.position = position;
        this.type = type;
        this.isFog = isFog;
    }
}
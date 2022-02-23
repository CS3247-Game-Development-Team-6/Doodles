using UnityEngine;

public enum CellType {
    // Can change name 
    WALK,
    PLANT,
    NONE
}


/** To hold relevant gameObjects, attributes etc.
 */
public class Cell {
    // index = (row, col)
    private Vector2Int index;
    private Vector3 position;
    private CellType type;
    private bool isFog;

    public Vector3 Position {
        get { return position; }
    }

    public Cell() {
        this.type = CellType.NONE;
    }

    public Cell(Vector2Int index, Vector3 position) {
        this.index = index;
        this.position = position;
        this.type = CellType.NONE;
    }

    public Cell(Vector2Int index, Vector3 position, CellType type, bool isFog) {
        this.index = index;
        this.position = position;
        this.type = type;
        this.isFog = isFog;
    }
}

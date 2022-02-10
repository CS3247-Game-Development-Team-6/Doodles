using UnityEngine;


/** To hold relevant gameObjects, attributes etc.
 */
public class Cell {
    private Vector2Int index;
    private Vector3 position;

    public Vector3 Position {
        get { return position; }
    }

    public Cell() { }

    public Cell(Vector2Int index, Vector3 position) {
        this.index = index;
        this.position = position;
    }
}

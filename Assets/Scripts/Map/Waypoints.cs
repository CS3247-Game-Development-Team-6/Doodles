using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] points;
    public Map map;
    
    /* Assigns the array of waypoints.
     */
    void Awake() {
        // assign array waypoints
        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++) {
            points[i] = transform.GetChild(i);
        }
    }

    /* Get the cell from world position of each waypoint, and sets the map's waypoints.
     * Must be executed after cells have been initialized (in Map.Awake())
     */
    private void Start() {
        Cell[] waypointCells = new Cell[points.Length];
        int n = waypointCells.Length;
        for (int i = 0; i < n; i++) {
            Transform waypoint = points[i];
            if (Physics.Raycast(waypoint.position, Vector3.down, out RaycastHit hit, Mathf.Infinity)) {
                // if the waypoint hits the plane right below
                if (hit.transform.Equals(map.mapBase.transform)) {
                    waypointCells[i] = map.GetCellFromWorldPosition(hit.point);
                }
            }
        }
        map.SetWaypoints(waypointCells);
    }

    /* Red markers indicate waypoints, green marker indicates base.
     */
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        int n = transform.childCount;
        for (int i = 0; i < n - 1; i++) {
            Gizmos.DrawSphere(transform.GetChild(i).position, 0.2f);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.GetChild(n - 1).position, 0.2f);
    }

}

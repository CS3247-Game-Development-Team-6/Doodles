using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] points;
    public Map map;
    
    void Awake() {
        // assign array waypoints
        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }

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

}

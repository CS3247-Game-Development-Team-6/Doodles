using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] points;
    public Transform[] waypointsPosition;
    public MapGenerator mapGen;
    public WaveSpawner waveSpawner;
    private Vector3 offset = new Vector3(0, 0.3f, 0);
    private bool waypointsActive = false;

    private void Start() {
        waveSpawner = FindObjectOfType<WaveSpawner>();
    }

    public void SetWaypoints(List<Cell> waypointCells, GameObject waypointPrefab) {
        waypointsPosition = new Transform[waypointCells.Count];
        GameObject prevWaypoint = null;
        for (int i = 0; i < waypointCells.Count; i++) {
            // Debug.Log($"waypoint {waypointCells[i].Index}");
            waypointsPosition[i] = waypointCells[i].tile.transform;
            GameObject waypoint = Instantiate(waypointPrefab, transform, true);
            waypoint.name = $"Waypoint {i}";
            if (waypoint != null) {
                waypoint.transform.position = i == waypointCells.Count - 1
                    ? changeEnemyTargetWaypoint(waypointCells[i].position, prevWaypoint)
                    : waypointCells[i].position;
            }
            prevWaypoint = waypoint;
        }
    }

    public void ActivateLocalWaypoints() {
        Debug.Log($"Activated {name}");
        points = waypointsPosition;

        waveSpawner.spawnPoint = points[0];
        waypointsActive = true;
    }
    public void DeactivateLocalWaypoints() {
        Debug.Log($"Deactivated {name}");
        waypointsActive = false;
    }

    private Vector3 changeEnemyTargetWaypoint(Vector3 basePosition, GameObject closestWaypoint) {
        if (closestWaypoint == null) return basePosition;

        var delta = basePosition - closestWaypoint.transform.position;

        delta = delta.normalized;
        delta *= 0.5f;  // this moves the waypoint 0.5 units towards the closest waypoint. can be in-/decreased.

        return basePosition - delta;
    }


    public void ActivateWaypoints() {
        Debug.Log($"Activated {name}");
        // assign array waypoints
        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            transform.GetChild(i).position += offset;
            points[i] = transform.GetChild(i);
        }

        waveSpawner.GetComponent<WaveSpawner>().spawnPoint = points[0];
        waypointsActive = true;
    }

    /* Red markers indicate waypoints, green marker indicates base.
     */
    private void OnDrawGizmos()
    // the gizmos are drawn after waypoints have actually been added
    // this is done to avoid the error when we try to draw gizmos before we have waypoints placed.
    {
        if (!waypointsActive) return;
        Gizmos.color = Color.red;
        for (int i = 0; i < points.Length; i++) {
            Gizmos.DrawSphere(points[i].position, 0.2f);
        }
    }
}

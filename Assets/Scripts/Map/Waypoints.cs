using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] points;
    public MapGenerator map;
    public GameObject gameMaster;
    private Vector3 offset = new Vector3(0, 0.3f, 0);
    private bool waypointsActive = false;


    public void ActivateWaypoints()
    {
        // assign array waypoints
        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            transform.GetChild(i).position += offset;
            points[i] = transform.GetChild(i);
        }

        gameMaster.GetComponent<WaveSpawner>().spawnPoint = points[0];
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
        int n = transform.childCount;
        for (int i = 0; i < n - 1; i++)
        {
            Gizmos.DrawSphere(transform.GetChild(i).position, 0.2f);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.GetChild(n - 1).position, 0.2f);
    }
}

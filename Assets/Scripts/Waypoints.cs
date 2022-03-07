using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] points;
    public Map map;
    public GameObject gameMaster;

    
    public void ActivateWaypoints()
    {
        // assign array waypoints
        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++) {
            points[i] = transform.GetChild(i);
        }

        gameMaster.GetComponent<WaveSpawner>().spawnPoint = points[0];

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

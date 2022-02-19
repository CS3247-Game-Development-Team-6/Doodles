using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10f;

    private Transform target;
    private int waypointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // first target, which is first waypoint in Waypoints
        target = Waypoints.points[0];
    }

    // Update is called once per frame
    void Update()
    {
        // movement direction to the target waypoint
        Vector3 direction = target.position - transform.position;
        
        // delta time is time passed since last frame
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.points.Length - 1) 
        {
            Destroy(gameObject);
            return;
        }
        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }
}

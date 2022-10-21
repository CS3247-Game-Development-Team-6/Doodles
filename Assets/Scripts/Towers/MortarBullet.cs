using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarBullet : Bullet {

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 previousPosition;
    private float parabolaHeight = 5f;
    private float shotDuration = 2f; // duration the shot is in the air in seconds
    private float time = 0f;

    void Start() {
        startPosition = this.transform.position;
        previousPosition = this.transform.position;
    }

    public void RegisterTargetPosition(Vector3 targetPosition) { // used to register a static position; no homing
        endPosition = targetPosition;
    }

    // Update is called once per frame
    protected override void Update() {
        time += Time.deltaTime;
        
        // endPosition = target.position; // Constantly home in on enemy position

        Vector3 dir = endPosition - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame) { // check if reached target position
            HitTarget();
        }

        // transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        // transform.LookAt(target);

        Vector3 movementDirection = transform.position - previousPosition;

        this.transform.rotation = Quaternion.LookRotation(movementDirection); // set rotation of mortar shell based on movement direction
        
        previousPosition = transform.position; // record previous position for calculating movement direction
        transform.position = MathParabola.Parabola(startPosition, endPosition, parabolaHeight, time / shotDuration);
    }
}

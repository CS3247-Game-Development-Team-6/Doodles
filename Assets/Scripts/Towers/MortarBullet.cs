using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarBullet : Bullet {

    private Vector3 startPosition;
    private float parabolaHeight = 50f;
    private float shotDuration = 2f;
    private float time = 0f;

    void Start() {
        startPosition = this.transform.position;
    }

    // Update is called once per frame
    protected override void Update() {
        time += Time.deltaTime;

        if (target == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame) {
            HitTarget();
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);

        transform.position = MathParabola.Parabola(startPosition, target.position, parabolaHeight, time / shotDuration);
    }
}

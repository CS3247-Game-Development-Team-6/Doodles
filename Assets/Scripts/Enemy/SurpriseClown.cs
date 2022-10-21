using UnityEngine;

public class SurpriseClown : Enemy {
    private bool dieOnBase;
    private float range = 0.5f;
    private Transform newTarget;
    private static string playerWallTag = "PlayerWall";

    public override void Start() {
        base.Start();
        dieOnBase = enemyInfo.dieOnBase;
    }

    protected override void Update() {
        FindBarrier();
        if (newTarget) target = newTarget;
        base.Update();
    }

    private void FindBarrier() {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(playerWallTag);
        (GameObject, float) result = FindNearestTarget(targets);
        GameObject nearestTarget = result.Item1;
        float shortestDistance = result.Item2;

        if (nearestTarget != null && nearestTarget.gameObject.tag == playerWallTag && shortestDistance <= range) {
            newTarget = nearestTarget.transform;
        } else {
            newTarget = null;
        }
    }

    private (GameObject, float) FindNearestTarget(GameObject[] _targets) {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestTarget = null;
        foreach (GameObject tempTarget in _targets) {
            float distanceToPlayer = Vector3.Distance(transform.position, tempTarget.transform.position);
            if (distanceToPlayer < shortestDistance) {
                shortestDistance = distanceToPlayer;
                nearestTarget = tempTarget;
            }
        }
        return (nearestTarget, shortestDistance); ;
    }

    public override void EndPath() {
        if (dieOnBase) base.Die();

        base.EndPath();
    }
}

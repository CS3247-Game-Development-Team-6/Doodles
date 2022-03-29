using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    // player or base
    private Transform target;
    
    // can be customized
    [Header("Attributes")]
    public float range = 1f;
    public float fireRate = 1f;
    private float fireCountDown = 0f;

    // when shooting, stop enemy movement
    public bool isShooting = false;

    [Header("Setup Fields")]
    public string playerTag = "Player";
    public string baseTag = "Base";

    public Transform partToRotate;
    public float rotationSpeed = 10f;

    public GameObject model;
    public Animator animator;
    public Transform rangeCenter;

    public GameObject bulletPrefab;
    public Transform firePoint;


    void Start()
    {
        InvokeRepeating ("UpdateTarget", 0f, 0.5f);
        model = transform.GetChild(2).gameObject;
        animator = model.GetComponent<Animator>();
        rangeCenter = transform.GetChild(3).gameObject.transform;

        fireCountDown = 1f / fireRate;
    }

    // dont need to find target every frame
    void UpdateTarget() {
        
        GameObject[] targets = GameObject.FindGameObjectsWithTag(playerTag);
        GameObject[] bases = GameObject.FindGameObjectsWithTag(baseTag);

        (GameObject, float) result = FindNearestTarget(targets, bases);
        GameObject nearestTarget = result.Item1;
        float shortestDistance = result.Item2;
        
        if (nearestTarget != null && shortestDistance <= range)
        {
            target = nearestTarget.transform;
        }
        else 
        {
            target = null;
        }
    }


    // _targets are player
    // _bases are multiple bases
    // return tuples of either nearest player/base and shortest distance to the enemy
    (GameObject, float) FindNearestTarget(GameObject[] _targets, GameObject[] _bases) 
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestTarget = null;

        foreach (GameObject tempTarget in _targets) 
        {
            float distanceToPlayer = Vector3.Distance(rangeCenter.position, tempTarget.transform.position);
            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearestTarget = tempTarget;
            }
        }

        foreach (GameObject tempTarget in _bases) 
        {
            // range from the model
            float distanceToPlayer = Vector3.Distance(rangeCenter.position, tempTarget.transform.position);
            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearestTarget = tempTarget;
            }
        }

        return (nearestTarget, shortestDistance);;
    }
    

    void Update()
    {
        if (target == null) {

            isShooting = false;
            return;          
        }

        isShooting = true;

        // rotate enemy using quaternion
        Vector3 dir = target.position - transform.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookAtRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        
        partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        model.transform.rotation = Quaternion.Euler(model.transform.rotation.x, rotation.y, rotation.z);

        if (fireCountDown <= 0f) 
        {
            StartCoroutine(Shoot());

            fireCountDown = 1f / fireRate;
        }

        fireCountDown -= Time.deltaTime;

    }

    // shoot according to firecountdown timer
    IEnumerator Shoot () {
        // animate
        animator.SetTrigger("shoot");

        // wait for animation ends
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            yield return null;

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;

        GameObject bulletGO = (GameObject) Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        EnemyBullet bullet = bulletGO.GetComponent<EnemyBullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
        
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rangeCenter.position, range);
    }
}

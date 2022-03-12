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

    [Header("Setup Fields")]
    public string playerTag = "Player";

    public Transform partToRotate;
    public float rotationSpeed = 10f;

    public GameObject bulletPrefab;
    public Transform firePoint;


    void Start()
    {
        InvokeRepeating ("UpdateTarget", 0f, 0.5f);
    }

    // dont need to find target every frame
    void UpdateTarget() {
        //TODO: attack base
        
        GameObject[] targets = GameObject.FindGameObjectsWithTag(playerTag);

        float shortestDistance = Mathf.Infinity;
        GameObject nearestTarget = null;
        foreach (GameObject tempTarget in targets) 
        {
            float distanceToPlayer = Vector3.Distance(transform.position, tempTarget.transform.position);
            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearestTarget = tempTarget;
            }
        }

        if (nearestTarget != null && shortestDistance <= range)
        {
            target = nearestTarget.transform;
        }
        else 
        {
            target = null;
        }
    }
    

    void Update()
    {
        if (target == null) 
            return;
        
        // rotate enemy using quaternion
        Vector3 dir = target.position - transform.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookAtRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

        if (fireCountDown <= 0f) 
        {
            Shoot();
            fireCountDown = 1f / fireRate;
        }

        fireCountDown -= Time.deltaTime;

    }

    void Shoot () {
        GameObject bulletGO = (GameObject) Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        EnemyBullet bullet = bulletGO.GetComponent<EnemyBullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

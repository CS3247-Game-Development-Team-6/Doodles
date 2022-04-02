using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;

    [Header("Attribute")]
    public float range = 15f;
    public Transform rotationBase;
    public float rotationSpeed = 10f;
    [SerializeField] private float cost = 10f;
    [SerializeField] private float swapElementCost = 30f;
    public float upgradeCost = 10f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public float fireRate = 1f;
    private float fireCountDown = 0f;
    public GameObject bulletPrefab;
    public Transform firePoint;


    public float Cost {
        get { return cost; }
    }

    public float GetSwapElementCost()
    {
        return swapElementCost;
    }

    // Start is called before the first frame update
    void Start()
    {

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies) 
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else 
        {
            target = null;
        }
    }

    void Shoot() 
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); ;
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) 
        {
            return;
        }

        // Enemy target lock on 
        Vector3 dir = target.position - transform.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(rotationBase.rotation, lookAtRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        rotationBase.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (fireCountDown <= 0f) 
        {
            Shoot();
            fireCountDown = 1f / fireRate;
        }

        fireCountDown -= Time.deltaTime;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range/3);
    }
}

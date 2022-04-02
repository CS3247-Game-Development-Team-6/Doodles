using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Turret : MonoBehaviour
{
    //private Transform target;

    [Header("Attribute")]
    public float range = 15f;
    public Transform rotationBase;
    public float rotationSpeed = 10f;
    [SerializeField] private float cost = 10f;
    private float swapElementCost = 30f;
    public float upgradeCost = 10f;
    [SerializeField] private int numberOfBulletsToFire = 12;
    [SerializeField] private bool isAoeTurret;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public float fireRate = 1f;
    private float fireCountDown = 0f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    
    private bool haveTarget;
    private Transform[] targetTransforms;
    [SerializeField] private GameObject cube;

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
        
        targetTransforms = isAoeTurret ? GetTargetTransforms() : new Transform[1];
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        float shortestDistance = Mathf.Infinity;
        
        GameObject nearestEnemy = null;
        haveTarget = false;
        targetTransforms[0] = null;
        
        foreach (GameObject enemy in enemies) 
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (!isAoeTurret && distanceToEnemy < shortestDistance 
                || isAoeTurret && distanceToEnemy <= range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
                haveTarget = true;

                // for AoE turret we only care if there is any enemy within range,
                // so if it is at least one, we can break
                if (isAoeTurret) 
                    break;  
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            targetTransforms[0] = nearestEnemy.transform;
        }
    }

    void Shoot() 
    {
        foreach (var targetTransform in targetTransforms) 
        {
            GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); ;
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            
            if (bullet != null)
            {
                bullet.Seek(targetTransform, isAoeTurret);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetTransforms[0] == null) 
        {
            return;
        }
        
        if (!isAoeTurret) 
        {
            // Enemy target lock on 
            Vector3 dir = targetTransforms[0].position - transform.position;
            Quaternion lookAtRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(rotationBase.rotation, lookAtRotation, Time.deltaTime * rotationSpeed).eulerAngles;
            rotationBase.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }

        if (fireCountDown <= 0f) 
        {
            Shoot();
            fireCountDown = 1f / fireRate;
        }

        fireCountDown -= Time.deltaTime;

    }
    
    private Transform[] GetTargetTransforms()
    {
        Transform[] arrOfTransforms = new Transform[numberOfBulletsToFire];
        for (int bulletNumber = 0; bulletNumber < numberOfBulletsToFire; bulletNumber++)
        {
            var aimingPointPosition = firePoint.position;
            var angleOfFire = bulletNumber * (Math.PI / (numberOfBulletsToFire / 2));

            (float x, float z) = polarCoordinates(range, angleOfFire);

            aimingPointPosition.x += x;
            aimingPointPosition.z += z;

            GameObject circleTarget = Instantiate(cube, aimingPointPosition, Quaternion.identity);
            arrOfTransforms[bulletNumber] = circleTarget.transform;
        }

        return arrOfTransforms;
    } 
    
    private (float, float) polarCoordinates(float radius, double angle)
    {
        float x = (float) (radius * Math.Cos(angle));
        float y = (float) (radius * Math.Sin(angle));

        return (x, y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

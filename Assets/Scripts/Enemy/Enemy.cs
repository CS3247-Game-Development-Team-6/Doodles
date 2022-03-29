using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{

    [SerializeField] private float speed;               // Serialized for debugging purposes

    [SerializeField] private float initSpeed = 1f;

    [SerializeField] private float health;              // Serialized for debugging purposes

    [SerializeField] private float initHealth = 100;

    [SerializeField] private int defense;

    [SerializeField] private int initDefense = 10;

    [SerializeField] private float inkGained = 1f;


    [SerializeField] private GameObject deathEffect;

    // Flags for status effects

    private bool isScorched = false;
    private bool isChilled = false;
    private bool isDrenched = false;
    private bool isScalded = false;
    private bool isFrozen = false;
    private bool isWeakened = false;
   
    public bool isInFog = true;
    private MeshRenderer ballMeshRenderer;
    private Transform ballParentTransform;

    private Transform target;
    private int waypointIndex = 0;

    // to reduce bullet damage
    private GameObject bulletPrefab;

    [Header("Unity Stuff")]
    public Image healthBar;
    
    public MapGenerator map;

    public void TakeDamage(float amount)
    {
        health = health - amount + defense;

        // float number between 0 and 1
        healthBar.fillAmount = health / initHealth;

        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeDot(float amount)
    {
        health = health - amount;

        // float number between 0 and 1
        healthBar.fillAmount = health / initHealth;

        if (health <= 0)
        {
            Die();
        }
    }

    public void ReduceSpeed(float slowAmount)
    {
        speed = initSpeed * slowAmount;
    }

    public void RestoreSpeed()
    {
        speed = initSpeed;
    }

    public void ReduceAttack(int atkDecreAmount)
    {
        bulletPrefab.GetComponent<EnemyBullet>().ReduceBulletDamage(atkDecreAmount);
    }

    public void RestoreAttack()
    {
        bulletPrefab.GetComponent<EnemyBullet>().RestoreBulletDamage();
    }

    public void ReduceDefense(int defDecreAmount)
    {
        defense = initDefense - defDecreAmount;
    }

    public void RestoreDefense()
    {
        defense = initDefense;
    }

    public void setScorched(bool b)
    {
        isScorched = b;
    }

    public bool getScorched()
    {
        return isScorched;
    }

    public void setChilled(bool b)
    {
        isChilled = b;
    }

    public bool getChilled()
    {
        return isChilled;
    }

    public void setDrenched(bool b)
    {
        isDrenched = b;
    }

    public bool getDrenched()
    {
        return isDrenched;
    }

    public void setScalded(bool b)
    {
        isScalded = b;
    }

    public bool getScalded()
    {
        return isScalded;
    }

    public void setFrozen(bool b)
    {
        isFrozen = b;
    }

    public bool getFrozen()
    {
        return isFrozen;
    }

    public void setWeakened(bool b)
    {
        isWeakened = b;
    }

    public bool getWeakened()
    {
        return isWeakened;
    }

    public bool getInFog()
    {
        return isInFog;
    }

    void Die ()
    {
        // add ink
        Player playerScript = GameObject.Find("Player").GetComponent<Player>();
        playerScript.AddInk(inkGained);

        // for new wave
        WaveSpawner.numEnemiesAlive--;
        
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        Destroy(gameObject);
  
    }

    void Start()
    {
        // Initialize Stats
        health = initHealth;
        speed = initSpeed;
        defense = initDefense;
        bulletPrefab = GetComponentInParent<EnemyShooting>().bulletPrefab;

        ballMeshRenderer = gameObject.GetComponent<MeshRenderer>();
        ballParentTransform = gameObject.transform;

        // first target, which is first waypoint in Waypoints
        target = Waypoints.points[0];
    }


    void Update()
    {
        if (GetComponent<EnemyShooting>().isShooting) {
            // stop movement
            return;
        }

        // movement direction to the target waypoint
        Vector3 direction = target.position - transform.position;
        
        // delta time is time passed since last frame
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
        
        // enemy is visible if not in fog, hence its visibility is the negation of the isInFog bool.
        setEnemyVisibility(!isInFog);
    }

    void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.points.Length - 1) 
        {
            EndPath();
            return;
        }
        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }

    // previously was enemy attack the base and destroy
    void EndPath()
    {
        // do nothing
    }
    
    /*
     * When entering fog, the enemy is in the fog
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fog"))
        {
            isInFog = true;
        }
    }

    /*
     * When moving from fog to fog, the enemy is still in the fog
     */
    private void OnTriggerStay(Collider other)
    {
         if (other.CompareTag("Fog"))
         {
             isInFog = true;
         }
    }

    /*
     * When the enemy leaves a fog and does not immediately go into a new fog block, the boolean is set to false.
     */
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fog"))
        {
            isInFog = false;
        }
    }

    private void setEnemyVisibility(bool isVisible)
    {
        ballMeshRenderer.enabled = isVisible;
        foreach (Transform childrenTransform in ballParentTransform)
        {
            childrenTransform.gameObject.SetActive(isVisible);
        }
    }

}

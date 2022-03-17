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

    [SerializeField] private int attackBaseDmg;         // Serialized for debugging purposes

    [SerializeField] private int initAttack = 50;

    [SerializeField] private int defense;

    [SerializeField] private int initDefense = 10;

    [SerializeField] private int inkGained = 50;


    [SerializeField] private GameObject deathEffect;

    // Flags for status effects
    [SerializeField] private bool isScorched = false;   // Serialized for debugging purposes
    [SerializeField] private bool isChilled = false;    // Serialized for debugging purposes
    [SerializeField] private bool isDrenched = false;   // Serialized for debugging purposes
    [SerializeField] private bool isScalded = false;   // Serialized for debugging purposes
    [SerializeField] private bool isFrozen = false;   // Serialized for debugging purposes
    [SerializeField] private bool isWeakened = false;   // Serialized for debugging purposes

    private Transform target;
    private int waypointIndex = 0;

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
        attackBaseDmg = initAttack - atkDecreAmount;
    }

    public void RestoreAttack()
    {
        attackBaseDmg = initAttack;
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

    // enemy die by physical damage
    void Die ()
    {
        // TODO: add ink

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
        attackBaseDmg = initAttack;
        defense = initDefense;

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

}

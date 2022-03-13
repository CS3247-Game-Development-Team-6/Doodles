using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    
    [SerializeField] private float initHealth = 100;

    [SerializeField] private int inkGained = 50;

    [SerializeField] private int attackBaseDmg = 50;

    [SerializeField] private GameObject deathEffect;

    private float health;
    private Transform target;
    private int waypointIndex = 0;

    [Header("Unity Stuff")]
    public Image healthBar;

    public void TakeDamage(int amount)
    {
        health -= amount;

        // float number between 0 and 1
        healthBar.fillAmount = health / initHealth;

        if (health <= 0)
        {
            Die();
        }
    }

    // enemy die by physical damage
    void Die ()
    {
        // TODO: add ink
        
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        Destroy(gameObject);
    }

    void Start()
    {
        health = initHealth;

        // first target, which is first waypoint in Waypoints
        target = Waypoints.points[0];
    }


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
            EndPath();
            return;
        }
        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }

    // enemy attack the base and destroy
    void EndPath()
    {
        // decrease base hp
        Base.receiveDmg(attackBaseDmg);

        Destroy(gameObject);
    }
}
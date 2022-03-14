using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// similar to tower bullet, but diff target
public class EnemyBullet : MonoBehaviour
{
    private Transform target;
    public float speed = 70f;
    [SerializeField] private int bulletDamage = 10;

    public void Seek(Transform _target) 
    { 
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) 
        { 
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        
        // hit 
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget() 
    {
        
        Damage(target);

        Destroy(gameObject);    // destroys the bullet
    }

    void Damage(Transform _target) {
        if (_target.CompareTag("Player"))
        {
            Debug.Log("player take damage");
            //_target.GetComponent<PlayerHealth>().TakeDamage(bulletDamage);
        }

        if (_target.CompareTag("Base"))
        {
            //Debug.Log("base take damage");
            Base.receiveDmg(bulletDamage);
        }
    }
}

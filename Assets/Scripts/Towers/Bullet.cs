using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Transform target;
    public float speed = 70f;
    public GameObject impactEffect;
    [SerializeField] private int bulletDamage;

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
        
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget() 
    {
        GameObject impactEffectParticle = (GameObject) Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(impactEffectParticle, 2f);

        if (target.CompareTag("Enemy"))
        {
            target.GetComponent<Enemy>().TakeDamage(bulletDamage);
        }

        Destroy(gameObject);    // destroys the bullet
    }
}
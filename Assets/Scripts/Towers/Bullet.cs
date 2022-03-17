using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Transform target;
    public float speed = 70f;
    public GameObject impactEffect;
    [SerializeField] private float bulletDamage;
    [SerializeField] private StatusEffectData _data;

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
        if (target.CompareTag("Enemy"))
        {
            var effectable = target.GetComponent<IEffectable>();
            if (effectable != null) effectable.ApplyEffect(_data);

            target.GetComponent<Enemy>().TakeDamage(bulletDamage);
        }

        Destroy(gameObject);    // destroys the bullet
    }
}

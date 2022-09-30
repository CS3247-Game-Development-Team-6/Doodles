using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GlobalDamage : GlobalEffect
{
  
    private float origMultiplier;
    [SerializeField] float damage=200.0f;

 
    public override void Activate()
    {
        Enemy[] allObjects = UnityEngine.Object.FindObjectsOfType<Enemy>();
        foreach (Enemy e in allObjects)
        {
            
                e.TakeDamage(damage, null);

        }
    }



    public override void Deactivate()
    {
    
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InkAugmentation : GlobalEffect
{
  
    private float origMultiplier;
    [SerializeField] float EffectTime=3.0f;

 
    public override void Activate()
    {

        SpellManager.instance.InkFactor = 10.0f;
    }



    public override void Deactivate()
    {
        SpellManager.instance.InkFactor = 1.0f;
    }
}
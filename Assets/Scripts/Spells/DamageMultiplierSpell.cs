using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DamageAugmentation : GlobalEffect
{
  

 
    public override void Activate() {

        // SpellManager.instance.DamageFactor = 10.0f;
    }



    public override void Deactivate()
    {
        // SpellManager.instance.DamageFactor = 1.0f;
    }
}
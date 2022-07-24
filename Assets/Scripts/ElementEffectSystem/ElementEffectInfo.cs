using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect")]
public class ElementEffectInfo : ScriptableObject
{
    public string Name;
    public float DOTAmount;
    public float TickSpeed;
    public float SlowAmount;        // Between 0 to 1
    public int AtkDecreAmount;
    public int DefDecreAmount;
    public float Lifetime;
    public string Element;

    public GameObject EffectParticles;
}

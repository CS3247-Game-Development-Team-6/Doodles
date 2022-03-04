using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHp : MonoBehaviour
{
    public static int hp;
    [SerializeField] private int startHp = 100;

    void Start()
    {
        hp = startHp;
    }

    public static void receiveDmg(int amount)
    {
        hp -= amount;
    }
}

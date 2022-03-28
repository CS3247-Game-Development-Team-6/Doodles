using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public static int startHp = 500;

    private static int hp;

    void Start()
    {
        hp = startHp;
    }

    public static void receiveDmg(int amount)
    {
        hp -= amount;
    }

    public static int getHp()
    {
        return hp;
    }

    public static int getHpPercentage() {
        return hp / startHp;
    }

    public static bool isHpLessThanHalf()
    {
        float percentage = (float) hp / startHp;

        return percentage < 0.25f;
       
    }
}

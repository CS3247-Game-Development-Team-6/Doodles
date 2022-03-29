using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private IndicatorUI playerBaseIndicator;
    public static int startHp = 500;

    private static int hp;

    void Start()
    {
        hp = startHp;
        playerBaseIndicator.maxValue = startHp;
        playerBaseIndicator.rawValue = hp;
    }

    private void Update() {
        playerBaseIndicator.rawValue = hp;
    }

    public static void receiveDmg(int amount)
    {
        hp -= amount;
    }

    public static int getHp()
    {
        return hp;
    }

    public static float getHpPercentage() {
        return (float)hp / startHp;
    }

    public static bool isHpLessThanHalf()
    {
        float percentage = (float) hp / startHp;

        return percentage < 0.25f;
       
    }
}

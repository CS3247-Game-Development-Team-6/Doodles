using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Ink;
    [SerializeField] private int startInk = 400;

    public static int Lives;
    [SerializeField] private int startLives = 20;

    void Start()
    {
        Ink = startInk;
        Lives = startLives;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHpUI : MonoBehaviour
{
    public Text baseHpText;

    // Update is called once per frame
    void Update()
    {
        if (BaseHp.isHpLessThanHalf()) {
            baseHpText.color= Color.red;
        }
        
        baseHpText.text = "BASE: " + BaseHp.getHp().ToString() + " HP";
    }
}

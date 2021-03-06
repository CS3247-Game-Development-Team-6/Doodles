using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHpUI : MonoBehaviour
{
    public Text baseHpText;
    public Image baseHpSlider;

    // Update is called once per frame
    void Update()
    {
        if (Base.isHpLessThanHalf()) {
            baseHpText.color= Color.red;
        }
        
        baseHpText.text = "BASE: " + Base.getHp().ToString() + " HP";
        baseHpSlider.fillAmount = Base.getHpPercentage();
    }
}

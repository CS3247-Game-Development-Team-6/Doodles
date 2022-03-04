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
        // if the game scales, may need to change implementation using coroutine
        baseHpText.text = "BASE: " + BaseHp.hp.ToString() + " HP";
    }
}

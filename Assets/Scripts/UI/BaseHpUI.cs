using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Base))]
public class BaseHpUI : MonoBehaviour {
    private Base startBase;
    public TextMeshProUGUI baseHpText;
    public Image baseHpSlider;

    private void Start() {
        startBase = GetComponent<Base>();
    }

    private void Update() {
        if (startBase.HpFract != baseHpSlider.fillAmount)
            Debug.Log(startBase.hp + " " + startBase.HpFract);
        if (baseHpText) {
            baseHpText.text = startBase.hp.ToString();
        }
        if (baseHpSlider) {
            baseHpSlider.fillAmount = startBase.HpFract;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SpellCooldownAbility2 : MonoBehaviour
{
    [SerializeField] private Image imageCooldown;
    [SerializeField] private TMP_Text textCooldown;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text textCost;
    [SerializeField] private TMP_Text notification;
    private bool isCooldown = false;
    private bool isInEffect = false;
    public float cooldownTime = 10.0f;
    private float cooldownTimer = 0.0f;
    public float effectTime = 3.0f;
    public float InkCost = 5.0f;
    private float effectTimer = 0.0f;
    [SerializeField] private InkManager PlayerObject;
    // Start is called before the first frame update
    void Start()
    {
        textCost.text = InkCost.ToString();
        button.onClick.AddListener(UseSpell);
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
        notification.text="";
    }

    // Update is called once per frame
    void Update()
    {
        if (isCooldown)
        {
            ApplyCooldown();
        }
        if (isInEffect)
        {
            ApplyEffect();
        }
        
    }
    void ApplyCooldown()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0.0f)
        {
            isCooldown = false;
            textCooldown.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0.0f;
        }
        else
        {
            textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            imageCooldown.fillAmount = cooldownTimer/cooldownTime;
        }
    }
    void ApplyEffect()
    {
        effectTimer -= Time.deltaTime;
        if (effectTimer < 0.0f)
        {
            isInEffect = false;
            SpellManager.instance.InkFactor = 1;
        }
    }
    IEnumerator sendNotification(string text,int time)
    {
        notification.text = text;
        yield return new WaitForSeconds(1);
        notification.text = "";
    }
    public void UseSpell()
    {
        if (isCooldown)
        {
            StartCoroutine(sendNotification("Spell is in cooldown",1));
            
        }
        else if (!PlayerObject.hasEnoughInk(InkCost))
        {
           
            StartCoroutine(sendNotification("Not enough ink", 1));

        }
        else
        {
            isCooldown = true;
            textCooldown.gameObject.SetActive(true);
            cooldownTimer = cooldownTime;
            SpellManager.instance.InkFactor = 10;
            isInEffect = true;
            effectTimer = effectTime;
            PlayerObject.ChangeInkAmount(-InkCost);
        }
    }
}

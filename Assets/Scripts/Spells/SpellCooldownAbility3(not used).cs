using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SpellCooldownAbility3 : MonoBehaviour
{
    [SerializeField] private Image imageCooldown;
    [SerializeField] private TMP_Text textCooldown;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text textCost;
    [SerializeField] private TMP_Text notification;
    [SerializeField] private float damage=200.0f;
    Vector3 position;
    public Image IndicatorRangeCircle;
    private bool isCooldown = false;
    public float cooldownTime = 10.0f;
    private float cooldownTimer = 0.0f;
    public float InkCost = 25.0f;
    [SerializeField] private InkManager PlayerObject;
    private bool InEffect = false;
    // Start is called before the first frame update
    void Start()
    {
        textCost.text = InkCost.ToString();
        button.onClick.AddListener(UseSpell);
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
        notification.text="";
        IndicatorRangeCircle.GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCooldown)
        {
            ApplyCooldown();
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
            PlayerObject.ChangeInkAmount(-InkCost);
            Enemy[] allObjects = UnityEngine.Object.FindObjectsOfType<Enemy>();
            foreach (Enemy e in allObjects)
            {
                e.TakeDamage(damage,null);
            }
            

        }
    }
}

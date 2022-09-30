using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable] public class RangeAbility : MonoBehaviour
{
    [SerializeField] private Image imageCooldown;
    [SerializeField] private TMP_Text textCooldown;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text textCost;
    [SerializeField] private TMP_Text notification;
    [SerializeField] private float damage=200.0f;
    
  
    private bool isCooldown = false;
    public float cooldownTime = 10.0f;
    private float cooldownTimer = 0.0f;
    public float InkCost = 25.0f;
    [SerializeField] private InkManager PlayerObject;
    private bool InEffect = false;
    public Image targetCircle;
    public Image indicatorRangeCircle;
    public Canvas ability2Canvas;
    Vector3 position;
    public float maxAbility2Distance;
    // Start is called before the first frame update
    void Start()
    {
       
        
       
        textCost.text = InkCost.ToString();
        button.onClick.AddListener(UseSpell);
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
        notification.text="";
        indicatorRangeCircle.gameObject.SetActive(false);
        targetCircle.gameObject.SetActive(false);



    }

    // Update is called once per frame
    void Update()
    {
        
       if (InEffect)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject != this.gameObject)
                {
                    
                    position = hit.point;
                }
            }
            var hitPosDir = (hit.point - PlayerObject.transform.position).normalized;
            float distance = Vector3.Distance(hit.point, PlayerObject.transform.position);
            distance = Mathf.Min(distance, maxAbility2Distance);
            
            var newHitPos = PlayerObject.transform.position + hitPosDir * distance;
            targetCircle.transform.position = (newHitPos);
            if (Input.GetMouseButtonDown(0))
            {
                InEffect = false;
                CastSpell();

            }
        }
     
        if (isCooldown)
        {
            ApplyCooldown();
            return;
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

    void CastSpell()
    {
        indicatorRangeCircle.gameObject.SetActive(false);
        targetCircle.gameObject.SetActive(false);
       
        
        Enemy[] allObjects = UnityEngine.Object.FindObjectsOfType<Enemy>();
        foreach (Enemy e in allObjects)
        {
            if (Vector3.Distance(targetCircle.transform.position, e.transform.position)<= maxAbility2Distance/2)
            {
                e.TakeDamage(damage,null);
            }
           
        }
        InEffect = false;
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
        else if (InEffect)
        {

        }
        else
        {
            targetCircle.gameObject.SetActive(true);
            indicatorRangeCircle.gameObject.SetActive(true); 
            isCooldown = true;
            textCooldown.gameObject.SetActive(true);
            cooldownTimer = cooldownTime;
            PlayerObject.ChangeInkAmount(-InkCost);
            InEffect = true;
           
            
            

        }
    }
}

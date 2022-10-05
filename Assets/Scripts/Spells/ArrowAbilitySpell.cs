using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable] public class ArrowAbility : MonoBehaviour
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
    public Image targetArrow;

    public Canvas ability2Canvas;
    Vector3 position;
    public float maxAbilityDistance;
    public float angle;
    // Start is called before the first frame update
    void Start()
    {


        targetArrow.gameObject.SetActive(false);
        textCost.text = InkCost.ToString();
        button.onClick.AddListener(UseSpell);
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
        notification.text="";
       



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
                position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }
            Quaternion transRot = Quaternion.LookRotation(position - PlayerObject.transform.position);

            targetArrow.rectTransform.localRotation = Quaternion.Euler(0, 0, -transRot.eulerAngles.y);
      
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
        targetArrow.gameObject.SetActive(false);
       
        
        Enemy[] allObjects = UnityEngine.Object.FindObjectsOfType<Enemy>();
        foreach (Enemy e in allObjects)
        {
               float angle1 = Vector3.Angle(targetArrow.transform.position, PlayerObject.transform.position);
               float angle2 = Vector3.Angle(e.transform.position, PlayerObject.transform.position);
            if (Mathf.Abs(angle1-angle2)<=angle)
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
            targetArrow.gameObject.SetActive(true);
            isCooldown = true;
            textCooldown.gameObject.SetActive(true);
            cooldownTimer = cooldownTime;
            PlayerObject.ChangeInkAmount(-InkCost);
            InEffect = true;
           
       
            

        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OverworkSpell : Spell {
    private static readonly Vector3 OFFSET = Vector3.up * 0.001f;
  
    [SerializeField] public float radiusOfEffect;
    [SerializeField] public float rangeRadius;
    [SerializeField] private float healingAmount;
    [SerializeField] private int healthNeed;
    [SerializeField] private GameObject indicatorPrefab;
   

    private Canvas indicator;

    private Image targetImage;
    private Image rangeImage;

    private Transform player;
    private bool isSearching;
    private SpellUI ui;

    private void Start() {
        player = FindObjectOfType<PlayerMovement>().transform;
     
    }

    private void Update() {
        if (!isSearching) return;
        if (!rangeImage || !targetImage) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            if (hit.collider.gameObject == this.gameObject) return;

            var hitPosDir = (hit.point - player.position).normalized;
            float distance = Vector3.Distance(hit.point, player.position);
            distance = Mathf.Min(distance, rangeRadius - radiusOfEffect);

            rangeImage.transform.position = player.position + OFFSET;
            targetImage.transform.position = player.position + hitPosDir * distance + OFFSET;

            // LEFT CLICK
            if (Input.GetMouseButtonDown(0)) {
                ChargeCost();

                PlayerHealth health = player.GetComponent<PlayerHealth>();
                health.TakeDamage(healthNeed);
                ui.ResetCooldownTimer();



                HealTower();
                
                StartCoroutine(Deactivate(ui));
            // RIGHT CLICK
            } else if (Input.GetMouseButtonDown(1)) {
                StartCoroutine(Deactivate(ui));
            }
        }
        
    }

    public override IEnumerator Activate(SpellUI ui) {
        indicator = Instantiate(indicatorPrefab).GetComponent<Canvas>();
        indicator.transform.position = player.position;
        rangeImage = indicator.transform.Find("Range").GetComponent<Image>();
        targetImage = indicator.transform.Find("Target").GetComponent<Image>();
        rangeImage.rectTransform.sizeDelta = new Vector2(rangeRadius*2, rangeRadius*2);
        targetImage.rectTransform.sizeDelta = new Vector2(radiusOfEffect*2, radiusOfEffect*2);
        isSearching = true;
        this.ui = ui;
        yield return new WaitForEndOfFrame();
    }
   
    public override IEnumerator Deactivate(SpellUI ui) {
        isSearching = false;
        Destroy(indicator.gameObject);
        yield return new WaitForSeconds(10);
    }

    public void HealTower() {
        Tower[] allObjects = FindObjectsOfType<Tower>();
        foreach (Tower t in allObjects) {
            if (Vector3.Distance(targetImage.transform.position, t.transform.position) <= radiusOfEffect) {
                t.IncreaseHealth(healingAmount);
            }
        }
       
    }

}

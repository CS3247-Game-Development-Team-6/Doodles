using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FireballSpell : Spell
{
    private static readonly Vector3 OFFSET = Vector3.up * 0.001f;

    [SerializeField] public float radiusOfEffect;
    [SerializeField] public float rangeRadius;
    [SerializeField] private float damage;
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private GameObject fireballPrefab;

    private Canvas indicator;
    private GameObject fireball;
    private Image targetImage;
    private Image rangeImage;

    private Transform player;
    private bool isSearching;
    private SpellUI ui;
    private bool inFalling;
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;

    }

    private void Update()
    {
        //TODO, this way is working, but probably could change to "standard way later, add collider to fireball and remove the isfalling flag
        if (inFalling)
        {
            if (player.position.y >= fireball.transform.position.y)
            {
                Attack();
                inFalling = false;
                Destroy(fireball);
                Destroy(indicator.gameObject);
            }
            return;

        }
        if (!isSearching) return;
        if (!rangeImage || !targetImage) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject == this.gameObject) return;

            var hitPosDir = (hit.point - player.position).normalized;
            float distance = Vector3.Distance(hit.point, player.position);
            distance = Mathf.Min(distance, rangeRadius - radiusOfEffect);

            rangeImage.transform.position = player.position + OFFSET;
            targetImage.transform.position = player.position + hitPosDir * distance + OFFSET;

            // LEFT CLICK
            if (Input.GetMouseButtonDown(0))
            {

                if (hit.collider.gameObject.name == "GroundTest")
                {

                    return;
                }
                SpellManager.instance.isCasting = false;
                fireball = Instantiate(fireballPrefab, targetImage.transform.position + Vector3.up * 10f, Quaternion.identity);
                inFalling = true;
                ChargeCost();
                ui.ResetCooldownTimer();
                StartCoroutine(Deactivate(ui));

            }
        }

    }
    public void cancelCast()
    {
        StartCoroutine(Deactivate(ui));
        SpellManager.instance.isCasting = false;
     
    }
    public override IEnumerator Activate(SpellUI ui)
    {
        SpellManager.instance.isCasting = true;
        // is this okay?
        indicator = Instantiate(indicatorPrefab).GetComponent<Canvas>();
        indicator.transform.position = player.position;
        rangeImage = indicator.transform.Find("Range").GetComponent<Image>();
        targetImage = indicator.transform.Find("Target").GetComponent<Image>();
        radiusOfEffect = Mathf.Min(rangeRadius * 0.8f, radiusOfEffect);
        rangeImage.rectTransform.sizeDelta = new Vector2(rangeRadius * 2, rangeRadius * 2);
        targetImage.rectTransform.sizeDelta = new Vector2(radiusOfEffect * 2, radiusOfEffect * 2);
        isSearching = true;
        this.ui = ui;
        yield return new WaitForEndOfFrame();
    }

    public override IEnumerator Deactivate(SpellUI ui)
    {
        isSearching = false;
        indicator.gameObject.SetActive(false);

        yield return new WaitForSeconds(10);


    }

    public void Attack()
    {

        Enemy[] allObjects = FindObjectsOfType<Enemy>();
        foreach (Enemy e in allObjects)
        {
            if (Vector3.Distance(targetImage.transform.position, e.transform.position) <= radiusOfEffect)
            {
                e.TakeDamage(damage, null);
            }
        }
    }

}

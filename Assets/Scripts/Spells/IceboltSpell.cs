using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IceboltSpell : Spell
{
    private static readonly Vector3 OFFSET = Vector3.up * 0.001f;

    [SerializeField] public float effectWidth;
    [SerializeField] public float effectLength;

    [SerializeField] private float damage;
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private GameObject fireballPrefab;

    private Canvas indicator;
    // private GameObject fireball;
    private Image targetImage;

    private Transform player;
    private bool isSearching;
    private SpellUI ui;
    private bool firstPress = true;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;

    }


    private void Update()
    {
        if (!isSearching) return;
        if (!targetImage) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {

            Vector3 position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            Quaternion transRot = Quaternion.LookRotation(player.position - position);
            targetImage.transform.position = player.position + OFFSET;
            targetImage.rectTransform.localRotation = Quaternion.Euler(0, 0, transRot.eulerAngles.y);
            // LEFT CLICK
            if (Input.GetMouseButtonDown(0))
            {
                if (firstPress)
                {
                    firstPress = false;
                    return;
                }
                SpellManager.instance.isCasting = false;
                ChargeCost();

                ui.ResetCooldownTimer();
                indicator.gameObject.SetActive(false);

                Attack();
                StartCoroutine(Deactivate(ui));
                Debug.Log("charging cost for icebolt");
                // RIGHT CLICK
            }
        }
    }

    public override IEnumerator Activate(SpellUI ui)
    {
        if (!isSearching)
        {
            firstPress = true;
            SpellManager.instance.isCasting = true;
            indicator = Instantiate(indicatorPrefab).GetComponent<Canvas>();
            indicator.transform.position = player.position;
            targetImage = indicator.transform.Find("Target").GetComponent<Image>();
            targetImage.rectTransform.sizeDelta = new Vector2(effectWidth, effectLength);
            isSearching = true;

            this.ui = ui;


            yield return new WaitForEndOfFrame();

        }
    }

    public override IEnumerator Deactivate(SpellUI ui)
    {
        isSearching = false;

        yield return new WaitForSeconds(10);

    }
    public void cancelCast()
    {
        if (!isSearching){
            return;
        }
        StartCoroutine(Deactivate(ui));
        SpellManager.instance.isCasting = false;
        Destroy(indicator.gameObject);
    }
    public void Attack()
    {
        //fireball = Instantiate(fireballPrefab, targetImage.transform.position + Vector3.up * 10f, Quaternion.identity);
        Enemy[] allObjects = FindObjectsOfType<Enemy>();
        foreach (Enemy e in allObjects)
        {
            float angle1 = Vector3.Angle(targetImage.transform.position, player.position);
            float angle2 = Vector3.Angle(e.transform.position, player.transform.position);
            if (Vector3.Distance(targetImage.transform.position, e.transform.position) <= effectLength && Mathf.Abs(angle1 - angle2) <= 25)
            {
                e.TakeDamage(damage, null);
            }
        }
        Destroy(indicator.gameObject);
    }

}


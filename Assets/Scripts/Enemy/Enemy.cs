using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/** 
 * Enemy's effect status caused by element reactions
 */
public enum EffectStatus {
    SCORCH, // fire
    CHILL, // ice
    DRENCH, // water
    SCALD, // fire + water
    FROZE, // ice + water
    WEAKEN, // fire + ice
    NONE // default
}

public class Enemy : MonoBehaviour {

    /**
     * enemy properties from EnemyInfo
     */
    private float speed;
    private float health;
    private int defense;
    private ElementInfo element;
    private float damageMultiplier;
    private float inkGained;
    private GameObject deathEffect;

    [SerializeField] private EnemyInfo enemyInfo;

    private EffectStatus effectStatus;

    public void setEffectStatus(EffectStatus status) {
        effectStatus = status;
    }
    public EffectStatus getEffectStatus() {
        return effectStatus;
    }
    public void removeEffectStatus() {
        effectStatus = EffectStatus.NONE;
    }

    /**
     * Visibility
     */
    private bool isInFog = true;
    private Transform ballParentTransform;
    private Canvas canvas;

    /**
     * Shoot target
     */
    private Transform target;

    /**
     * Rotation
     */
    private GameObject model;
    private Animator animator;

    /**
     * Translation
     */
    private int waypointIndex = 0;
    private int lastXCoord = 0;
    private int lastYCoord = 0;
    private Cell[,] cells;

    private InkManager inkManager;

    [Header("Unity Stuff")]
    public Image healthBar;
    public TMP_Text healthText;
    public GameObject damageText;

    /**
     * ExtraEffectManager
     */
    private ExtraEffectManager extraEffectManager;

    private void Awake() {
        inkManager = InkManager.instance;
    }

    /**
     * Immune to damage with same elements type from tower. Tower deals more damage to specific element type's enemy
     * 
     * Reference: https://drive.google.com/drive/folders/1Ck3jqkF_k5snVlAlZsA441pl4-DpjStC  
     */
    public void TakeDamage(float amount, ElementInfo bulletElement) {
        if (element == null || bulletElement == null) {
            ReduceHealth(amount);
            return;
        }

        if (element.type == bulletElement.type) {
            return;
        }

        if (element.weakness == bulletElement.type) {
            ReduceHealth(amount * damageMultiplier);
            return;
        }
        ReduceHealth(amount);
    }

    /*
     * Take account of enemy defense when reducing health
     */
    private void ReduceHealth(float amount) {
        if (defense < amount) {
            float temp = amount - defense;
            health -= temp;

            // update health bar, float number between 0 and 1
            healthBar.fillAmount = health / enemyInfo.health;

            DamageIndicator indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
            indicator.SetDamageTextFromFloat(temp);
        }
    }

    public void TakeDot(float amount) {
        health -= amount;

        // float number between 0 and 1
        healthBar.fillAmount = health / enemyInfo.health;

        DamageIndicator indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        indicator.SetDamageTextFromFloat(amount);
    }

    public void applyGel() {
        extraEffectManager.applyGel();
    }

    public float getSpeed() {
        return speed;
    }

    public void setSpeed(float speed) {
        this.speed = speed;
    }

    public void ReduceSpeed(float slowAmount) {
        speed = enemyInfo.speed * slowAmount;
        extraEffectManager.calculateGelSpeed();
    }

    public void RestoreSpeed() {
        speed = enemyInfo.speed;
        extraEffectManager.calculateGelSpeed();
    }

    public void ReduceAttack(int atkDecreAmount) {
        GetComponent<EnemyShooting>().ReduceBulletDamage(atkDecreAmount);
    }

    public void RestoreAttack() {
        GetComponent<EnemyShooting>().RestoreBulletDamage();
    }

    public void ReduceDefense(int defDecreAmount) {
        if (defDecreAmount > enemyInfo.defense) defense = 0;
        else defense = enemyInfo.defense - defDecreAmount;
    }

    public void RestoreDefense() {
        defense = enemyInfo.defense;
    }

    private void Die() {
        // add ink
        inkManager.ChangeInkAmount(inkGained);

        // for new wave
        WaveSpawner.numEnemiesAlive--;
        WaveSpawner.numEnemiesLeftInWave--;

        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        Destroy(gameObject);
    }

    void Start() {
        health = enemyInfo.health;
        speed = enemyInfo.speed;
        defense = enemyInfo.defense;
        element = enemyInfo.element;
        damageMultiplier = enemyInfo.damageMultiplier;
        inkGained = enemyInfo.inkGained;
        deathEffect = enemyInfo.deathEffect;
        effectStatus = EffectStatus.NONE;

        model = transform.GetChild(2).gameObject;
        animator = model.GetComponent<Animator>();
        canvas = transform.GetChild(0).GetComponent<Canvas>();
        ballParentTransform = gameObject.transform;

        // first target, which is first waypoint in Waypoints
        target = Waypoints.points[0];

        // get a reference to all cells for checking if a tile is fogged or not
        cells = GameObject.Find("Map").GetComponent<MapGenerator>().GetCells();
    
        // get reference to its ExtraEffectManager
        extraEffectManager = GetComponent<ExtraEffectManager>();
    }

    private void Update() {
        healthText.text = string.Format("{0}", health);

        if (health <= 0) {
            Die();
        }

        if (GetComponent<EnemyShooting>().isShooting) {
            if (getEffectStatus() == EffectStatus.FROZE) GetComponent<EnemyShooting>().enabled = false;
            else GetComponent<EnemyShooting>().enabled = true;
            // stop movement
            animator.SetBool("isWalking", false);
            return;
        }

        if (getEffectStatus() == EffectStatus.FROZE) {
            animator.SetBool("isWalking", false);
            return;
        }
        GetComponent<EnemyShooting>().enabled = true;
        animator.SetBool("isWalking", true);

        // movement direction to the target waypoint
        Vector3 direction = target.position - transform.position;

        if (direction != Vector3.zero) {
            // Do the rotation here
            Quaternion lookAtRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(model.transform.rotation, lookAtRotation, Time.deltaTime * 10f).eulerAngles;
            model.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        }

        // delta time is time passed since last frame
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        var currentPosition = transform.position;

        if (Vector3.Distance(currentPosition, target.position) <= 0.2f) {
            GetNextWaypoint();
        }

        int currentXCoord = Convert.ToInt32(Math.Floor(currentPosition.x));
        int currentYCoord = Convert.ToInt32(Math.Floor(currentPosition.z));

        if (currentXCoord != lastXCoord || currentYCoord != lastYCoord) {
            lastXCoord = currentXCoord;
            lastYCoord = currentYCoord;
            isInFog = GetCurrentTileFogged(currentXCoord, currentYCoord);
        }

        // enemy is visible if not in fog, hence its visibility is the negation of the isInFog bool.
        SetEnemyVisibility(!isInFog);
    }

    private bool GetCurrentTileFogged(int xCoord, int yCoord) {
        Cell cell = cells[yCoord, xCoord];
        return cell.isFog;
    }
    private void GetNextWaypoint() {
        if (waypointIndex >= Waypoints.points.Length - 1) {
            EndPath();
            return;
        }
        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }

    private void EndPath() {
        animator.SetBool("isWalking", false);
    }

    private void SetEnemyVisibility(bool isVisible) {
        foreach (Transform childrenTransform in ballParentTransform) {
            // disable canvas
            if (childrenTransform.name == canvas.name)
                childrenTransform.gameObject.SetActive(isVisible);

            // disable every renderers
            if (childrenTransform.name == model.name) {
                SkinnedMeshRenderer[] renderers = model.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (SkinnedMeshRenderer r in renderers) {
                    r.enabled = isVisible;
                }

                MeshRenderer[] meshRenderers = model.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer r in meshRenderers) {
                    r.enabled = isVisible;
                }
            }

        }
    }

}

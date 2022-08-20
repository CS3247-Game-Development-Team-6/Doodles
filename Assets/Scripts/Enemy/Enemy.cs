using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour {

    /**
     * enemy properties 
     */
    [SerializeField] private float speed;
    [SerializeField] private float initSpeed = 1f;
    [SerializeField] private float health;
    [SerializeField] private float initHealth = 100;
    [SerializeField] private int defense;
    [SerializeField] private int initDefense = 10;
    [SerializeField] private float inkGained = 1f;
    [SerializeField] private GameObject deathEffect;

    /** 
     * Effect status
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
    public EffectStatus effectStatus = EffectStatus.NONE;
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
    public bool isInFog = true;
    private Transform ballParentTransform;
    public Canvas canvas;

    /**
     * Shoot target
     */
    private Transform target;
    private GameObject bulletPrefab;

    /**
     * Rotation
     */
    public GameObject model;
    public Animator animator;

    /**
     * Translation
     */
    private int waypointIndex = 0;
    private int lastXCoord = 0;
    private int lastYCoord = 0;
    public MapGenerator map;
    private Cell[,] cells;

    private InkManager inkManager;

    [Header("Unity Stuff")]
    public Image healthBar;
    public TMP_Text healthText;

    private void Awake() {
        inkManager = InkManager.instance;
    }

    public void TakeDamage(float amount) {
        // Damage to be taken is higher than defense
        if (defense < amount) {
            health = health - amount + defense;
        }

        // float number between 0 and 1
        healthBar.fillAmount = health / initHealth;
    }

    public void TakeDot(float amount) {
        health = health - amount;

        // float number between 0 and 1
        healthBar.fillAmount = health / initHealth;

    }

    public void ReduceSpeed(float slowAmount) {
        speed = initSpeed * slowAmount;
    }

    public void RestoreSpeed() {
        speed = initSpeed;
    }

    public void ReduceAttack(int atkDecreAmount) {
        bulletPrefab.GetComponent<EnemyBullet>().ReduceBulletDamage(atkDecreAmount);
    }

    public void RestoreAttack() {
        bulletPrefab.GetComponent<EnemyBullet>().RestoreBulletDamage();
    }

    public void ReduceDefense(int defDecreAmount) {
        if (defDecreAmount > initDefense) defense = 0;
        else defense = initDefense - defDecreAmount;
    }

    public void RestoreDefense() {
        defense = initDefense;
    }

    void Die() {
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
        health = initHealth;
        speed = initSpeed;
        defense = initDefense;
        bulletPrefab = GetComponentInParent<EnemyShooting>().bulletPrefab;

        model = transform.GetChild(2).gameObject;
        animator = model.GetComponent<Animator>();
        canvas = transform.GetChild(0).GetComponent<Canvas>();
        ballParentTransform = gameObject.transform;

        // first target, which is first waypoint in Waypoints
        target = Waypoints.points[0];

        // get a reference to all cells for checking if a tile is fogged or not
        cells = GameObject.Find("Map").GetComponent<MapGenerator>().GetCells();
    }


    void Update() {
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
        setEnemyVisibility(!isInFog);

    }

    private bool GetCurrentTileFogged(int xCoord, int yCoord) {
        Cell cell = cells[yCoord, xCoord];
        return cell.isFog;
    }

    void GetNextWaypoint() {
        if (waypointIndex >= Waypoints.points.Length - 1) {
            EndPath();
            return;
        }
        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }

    void EndPath() {
        animator.SetBool("isWalking", false);
    }

    private void setEnemyVisibility(bool isVisible) {
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
            }

        }
    }

}

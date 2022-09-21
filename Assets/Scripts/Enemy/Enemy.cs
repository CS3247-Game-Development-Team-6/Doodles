using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Status {
    /**
     * Element status
     */
    SCORCH, // fire
    CHILL, // ice
    DRENCH, // water
    SCALD, // fire + water
    FROZE, // ice + water
    WEAKEN, // fire + ice

    /**
     * Unique status
     */
    INVUlNERABLE, // immune to everything
    NONE // default
}

/**
 * Main functionality: control movement, hp, death and visibility in fog
 */
public class Enemy : MonoBehaviour {

    /**
     * basic enemy properties
     */
    private float speed;
    private float health;
    private int defense;
    private float inkGained;
    private GameObject deathEffect;

    /**
     * Elemental
     */
    private ElementInfo element;
    private float damageMultiplier;

    /**
     * Invulnerability
     */
    private bool isInvulnerable;
    private float duration;
    private float cooldown;

    /**
     * Spawnable after death
     */
    private bool isSpawnable;
    private int spawnCount;
    private GameObject spawnPrefab;

    [SerializeField] private EnemyInfo enemyInfo;

    private Status status;

    public void SetStatus(Status _status) {
        status = _status;
    }
    public Status GetStatus() {
        return status;
    }
    public void RemoveStatus() {
        status = Status.NONE;
    }

    /**
     * Visibility
     */
    private bool isInFog = true;
    private Transform ballParentTransform;
    private Canvas canvas;
    private int lastXCoord = 0;
    private int lastYCoord = 0;
    private Cell[,] cells;

    /**
     * Rotation
     */
    private GameObject model;
    private Animator animator;

    /**
     * Translation
     */
    private int waypointIndex = 0;
    private Transform target;

    private InkManager inkManager;

    [Header("Unity Stuff")]
    public Image healthBar;
    public TMP_Text healthText;
    public GameObject damageText;

    private void Awake() {
        inkManager = InkManager.instance;
    }

    /**
     * Immune to damage with same elements type from tower. Tower deals more damage to specific element type's enemy
     * 
     * Reference: https://drive.google.com/drive/folders/1Ck3jqkF_k5snVlAlZsA441pl4-DpjStC  
     */
    public void TakeDamage(float amount, ElementInfo bulletElement) {
        if (GetStatus() == Status.INVUlNERABLE) {
            return;
        }

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
        if (GetStatus() == Status.INVUlNERABLE) {
            return;
        }

        health -= amount;

        // float number between 0 and 1
        healthBar.fillAmount = health / enemyInfo.health;

        DamageIndicator indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        indicator.SetDamageTextFromFloat(amount);
    }

    public void ReduceSpeed(float slowAmount) {
        speed = enemyInfo.speed * slowAmount;
    }

    public void RestoreSpeed() {
        speed = enemyInfo.speed;
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

    /**
     * offset to separate between mobs
     */
    private void SpawnWhenDeath(bool _isSpawnable, GameObject _prefab, Vector3 _spawnPosition, Vector3 _forwardVector, int _spawnCount) {
        if (_isSpawnable && _spawnCount > 0 && _prefab != null) {
            Vector3 minVector = -(_forwardVector);
            Vector3 maxVector = _forwardVector;

            for (int i = 0; i < _spawnCount; i++) {
                Vector3 spawnOffset = new Vector3(
                    UnityEngine.Random.Range(minVector.x, maxVector.x),
                    UnityEngine.Random.Range(minVector.y, maxVector.y),
                    UnityEngine.Random.Range(minVector.z, maxVector.z)
                );
                GameObject spawnGO = (GameObject)Instantiate(_prefab, _spawnPosition + spawnOffset, Quaternion.identity);

                /*
                 * set movement
                 */
                Enemy enemyScript = spawnGO.GetComponent<Enemy>();
                if (enemyScript != null) {
                    enemyScript.InitWaypoint(waypointIndex);
                }

                /*
                 * wave enemy number
                 */
                WaveSpawner.numEnemiesLeftInWave++;
                WaveSpawner.numEnemiesAlive++;
            }
        }
    }

    private void Die() {
        SpawnWhenDeath(isSpawnable, spawnPrefab, transform.position,
            (target.position - transform.position).normalized, spawnCount);

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
        isInvulnerable = enemyInfo.isInvulnerable;
        duration = enemyInfo.duration;
        cooldown = enemyInfo.cooldown;
        isSpawnable = enemyInfo.isSpawnable;
        spawnCount = enemyInfo.spawnCount;
        spawnPrefab = enemyInfo.spawnPrefab;
        target = Waypoints.points[waypointIndex];
        status = Status.NONE;

        model = transform.GetChild(2).gameObject;
        animator = model.GetComponent<Animator>();
        canvas = transform.GetChild(0).GetComponent<Canvas>();
        ballParentTransform = gameObject.transform;

        // get a reference to all cells for checking if a tile is fogged or not
        cells = GameObject.Find("Map").GetComponent<MapGenerator>().GetCells();
    }

    private void Update() {
        healthText.text = string.Format("{0}", health);

        if (health <= 0) {
            Die();
        }

        if (isInvulnerable && GetStatus() == Status.INVUlNERABLE) {
            if (duration <= 0f) {
                DisableInvulnerability();
            }
            duration -= Time.deltaTime;
        }

        if (isInvulnerable && GetStatus() != Status.INVUlNERABLE) {
            if (cooldown <= 0f) {
                EnableInvulnerability();
            }
            cooldown -= Time.deltaTime;
        }


        if (GetComponent<EnemyShooting>().isShooting) {
            if (GetStatus() == Status.FROZE) GetComponent<EnemyShooting>().enabled = false;
            else GetComponent<EnemyShooting>().enabled = true;
            // stop movement
            animator.SetBool("isWalking", false);
            return;
        }

        if (GetStatus() == Status.FROZE) {
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

        /**
         * visibility in fog
         */
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

    public void InitWaypoint(int index) {
        waypointIndex = index;
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

    private void EnableInvulnerability() {
        GetComponent<EffectManager>().RemoveEffect();
        SetStatus(Status.INVUlNERABLE);
        cooldown = enemyInfo.cooldown;

        // only for invulnerable enemy
        transform.GetChild(4).gameObject.SetActive(true);
    }

    private void DisableInvulnerability() {
        SetStatus(Status.NONE);
        duration = enemyInfo.duration;

        // only for invulnerable enemy
        transform.GetChild(4).gameObject.SetActive(false);
    }

}

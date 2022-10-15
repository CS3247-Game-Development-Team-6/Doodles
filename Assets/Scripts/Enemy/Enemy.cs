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
    INVULNERABLE, // immune to everything
    NONE // default
}


/**
 * Main functionality: control movement, hp, death and visibility in fog
 */
[RequireComponent(typeof(EnemyActiveEffects))]
public class Enemy : MonoBehaviour {
    private const float EPSILON = 0.02f;
    private const string CANVAS_NAME = "Canvas";
    private const string MODEL_NAME = "Model";
    private const string SPHERE_NAME = "Sphere"; // for invulnerable enemy

    /**
     * basic enemy properties
     */
    private float speed;
    private float health;
    private int defense;
    private float inkGained;
    private GameObject deathEffect;
    private float damageAugmentationFactor = 1.0f;

    /**
     * Elemental
     */
    private ElementInfo element;
    private float damageMultiplier;

    /**
     * Enemy Status
     */
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
     * Invulnerability
     */
    private bool isInvulnerable;
    private float duration;
    private float cooldown;

    /**
     * Spawnable after death
     */
    private bool spawnsOnDeath;
    private int spawnCount;
    private GameObject spawnPrefab;
    private GameObject spawnEffect;
    private bool bombCarry;

    [SerializeField] private EnemyInfo enemyInfo;

    /**
     * Visibility
     */
    private bool isInFog = true;
    private Transform ballParentTransform;
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
    private Transform target;
    public int waypointIndex = 0; // make public for a quick fix so that enemy dont attack base without reaching

    /**
     * EnemyActiveEffectsManager
     */
    public float speedMultiplier;
    private EnemyActiveEffects enemyActiveEffectsManager;

    public Waypoints waypoints { get; set; }
    public ChunkSpawner chunkSpawner { get; set; }
    public bool isShooting = false;
    private EnemyShooting shootingScript;
    private InkManager inkManager;

    [Header("Unity Stuff")]
    public Image healthBar;
    public TMP_Text healthText;
    public GameObject damageText;

    private void Awake() {
        inkManager = InkManager.instance;
        enemyActiveEffectsManager = GetComponent<EnemyActiveEffects>();
        shootingScript = GetComponent<EnemyShooting>();
    }

    /**
     * Immune to damage with same elements type from tower. Tower deals more damage to specific element type's enemy
     * 
     * Reference: https://drive.google.com/drive/folders/1Ck3jqkF_k5snVlAlZsA441pl4-DpjStC  
     */
    public void TakeDamage(float amount, ElementInfo bulletElement) {
        if (GetStatus() == Status.INVULNERABLE) {
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
        amount = amount * damageAugmentationFactor;
        if (defense < amount) {
            float temp = amount - defense;
            health -= temp;

            // update health bar, float number between 0 and 1
            healthBar.fillAmount = health / enemyInfo.health;

            //DamageIndicator indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
            //indicator.SetDamageTextFromFloat(temp);
        }
    }

    public void TakeDot(float amount) {
        if (GetStatus() == Status.INVULNERABLE) {
            return;
        }

        health -= amount;

        // float number between 0 and 1
        healthBar.fillAmount = health / enemyInfo.health;

        //DamageIndicator indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        //indicator.SetDamageTextFromFloat(amount);
    }
    public float GetDamamgeMultiplier() {
        return damageAugmentationFactor;
    }
    public void SetDamamgeMultiplier(float multiplyAmount) {
        damageAugmentationFactor = multiplyAmount;
    }
    public void ApplyEffect(IEnemyEffect effect) {
        StartCoroutine(enemyActiveEffectsManager.HandleEffect(effect));
    }

    public float getFinalSpeed() {
        return speed * speedMultiplier;
    }

    public void ReduceBaseSpeed(float slowAmount) {
        speed = enemyInfo.speed * slowAmount;
    }

    public void RestoreBaseSpeed() {
        speed = enemyInfo.speed;
    }

    public void ReduceAttack(int atkDecreAmount) {
        shootingScript.ReduceBulletDamage(atkDecreAmount);
    }

    public void RestoreAttack() {
        shootingScript.RestoreBulletDamage();
    }

    public int GetDefense() {
        return defense;
    }
    public void ReduceDefense(int defDecreAmount) {
        if (defDecreAmount > enemyInfo.defense) defense = 0;
        else defense = enemyInfo.defense - defDecreAmount;
    }

    public void SetDefense(int amount) {
        defense = amount;
    }

    public void RestoreDefense() {
        defense = enemyInfo.defense;
    }

    /**
     * offset to separate between mobs
     */
    private void SpawnWhenDeath(GameObject _prefab, Vector3 _spawnPosition, Transform target,
        int _spawnCount) {

        Vector3 minRange = target ? -(target.position - transform.position).normalized : new Vector3(0, 0, 0);
        Vector3 maxRange = target && target != waypoints.GetPoint(waypoints.Length - 1) ?
                    // current enemy is near base, to avoid spawning mobs into the base
                    (target.position - transform.position).normalized :
                    new Vector3(0, 0, 0);

        for (int i = 0; i < _spawnCount; i++) {
            Vector3 spawnOffset = new Vector3(
                UnityEngine.Random.Range(minRange.x, maxRange.x),
                UnityEngine.Random.Range(minRange.y, maxRange.y),
                UnityEngine.Random.Range(minRange.z, maxRange.z)
            );
            GameObject spawnGO = (GameObject)Instantiate(_prefab, _spawnPosition + spawnOffset, Quaternion.identity);

            if (spawnEffect) {
                // to spawn the effect following game object
                Instantiate(spawnEffect, spawnGO.transform.position, Quaternion.identity, spawnGO.transform);
            }

            /*
             * set movement
             */
            Enemy enemyScript = spawnGO.GetComponent<Enemy>();
            if (enemyScript != null) {
                enemyScript.InitSpawnWhenDeath(this.chunkSpawner, waypointIndex);
            }

            /*
             * wave enemy number
             */
            chunkSpawner.numEnemiesLeftInWave++;
            chunkSpawner.numEnemiesAlive++;
        }

    }

    private void Die() {
        if (spawnsOnDeath && spawnCount > 0 && spawnPrefab != null) {
            SpawnWhenDeath(spawnPrefab, transform.position,
                target,
                spawnCount
            );
        }


        // add ink
        inkManager.ChangeInkAmount(inkGained);

        // for new wave
        chunkSpawner.numEnemiesAlive--;
        chunkSpawner.numEnemiesLeftInWave--;

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
        spawnsOnDeath = enemyInfo.isSpawnable;
        spawnCount = enemyInfo.spawnCount;
        spawnPrefab = enemyInfo.spawnPrefab;
        spawnEffect = enemyInfo.spawnEffect;
        bombCarry = enemyInfo.isBombCarry;
        status = Status.NONE;

        model = transform.Find(MODEL_NAME).gameObject;
        animator = model.GetComponent<Animator>();
        ballParentTransform = gameObject.transform;
    }

    public void Init(ChunkSpawner chunkSpawner) {
        this.chunkSpawner = chunkSpawner;
        Chunk currChunk = chunkSpawner.GetComponent<Chunk>();
        cells = currChunk.cells;
        waypoints = currChunk.GetComponent<Waypoints>();
        target = waypoints.points[0];
        speedMultiplier = 1.0f;
        enemyActiveEffectsManager = GetComponent<EnemyActiveEffects>();
    }

    public void InitSpawnWhenDeath(ChunkSpawner chunkSpawner, int index) {
        this.chunkSpawner = chunkSpawner;
        Chunk currChunk = chunkSpawner.GetComponent<Chunk>();
        cells = currChunk.cells;
        waypoints = currChunk.GetComponent<Waypoints>();
        waypointIndex = index;
        target = waypoints.points[index];
        speedMultiplier = 1.0f;
        enemyActiveEffectsManager = GetComponent<EnemyActiveEffects>();
    }

    private void Update() {
        healthText.text = string.Format("{0:N0}", health);

        if (health <= 0) {
            Die();
        }

        if (isInvulnerable && GetStatus() == Status.INVULNERABLE) {
            if (duration <= 0f) {
                DisableInvulnerability();
            }
            duration -= Time.deltaTime;
        }

        if (isInvulnerable && GetStatus() != Status.INVULNERABLE) {
            if (cooldown <= 0f) {
                EnableInvulnerability();
            }
            cooldown -= Time.deltaTime;
        }

        if (isShooting || GetStatus() == Status.FROZE || !target) {
            animator.SetBool("isWalking", false);
            return;
        }

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
        transform.Translate(direction.normalized * getFinalSpeed() * Time.deltaTime, Space.World);

        var currentPosition = transform.position - chunkSpawner.transform.position;
        var targetPosition = target.position - chunkSpawner.transform.position;

        if (Vector3.Distance(currentPosition, targetPosition) <= EPSILON || target.CompareTag("Base") &&
            Vector3.Distance(currentPosition, targetPosition) <= 0.5f) {
            GetNextWaypoint();
        }

        // Col on x axis
        int col = Convert.ToInt32(Math.Floor(currentPosition.x));
        // Row on y axis
        int row = Convert.ToInt32(Math.Floor(currentPosition.z));


        if (row != lastYCoord || col != lastXCoord) {
            lastYCoord = row;
            lastXCoord = col;
            isInFog = GetCurrentTileFogged(row, col);
        }

        // enemy is visible if not in fog, hence its visibility is the negation of the isInFog bool.
        SetEnemyVisibility(!isInFog);
    }

    private bool GetCurrentTileFogged(int row, int col) {
        // is inverted
        Cell cell = cells[row, col];
        return cell.isFog;
    }

    private void GetNextWaypoint() {
        if (waypointIndex >= waypoints.Length - 1) {
            EndPath();
            return;
        }
        waypointIndex++;
        target = waypoints.GetPoint(waypointIndex);
    }

    private void EndPath() {
        if (bombCarry) {
            Die();
        }

        target = null;
        animator.SetBool("isWalking", false);
    }

    private void SetEnemyVisibility(bool isVisible) {
        foreach (Transform childrenTransform in ballParentTransform) {
            // disable canvas
            if (childrenTransform.name == CANVAS_NAME)
                childrenTransform.gameObject.SetActive(isVisible);

            // disable every renderers
            if (childrenTransform.name == MODEL_NAME || childrenTransform.name == SPHERE_NAME) {
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
        SetStatus(Status.INVULNERABLE);
        cooldown = enemyInfo.cooldown;

        // only for invulnerable enemy
        transform.Find(SPHERE_NAME).gameObject.SetActive(true);
    }

    private void DisableInvulnerability() {
        SetStatus(Status.NONE);
        duration = enemyInfo.duration;

        // only for invulnerable enemy
        transform.Find(SPHERE_NAME).gameObject.SetActive(false);
    }

}

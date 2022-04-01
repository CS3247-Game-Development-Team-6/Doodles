using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{

    [SerializeField] private float speed;               // Serialized for debugging purposes

    [SerializeField] private float initSpeed = 1f;

    [SerializeField] private float health;              // Serialized for debugging purposes

    [SerializeField] private float initHealth = 100;

    [SerializeField] private int defense;

    [SerializeField] private int initDefense = 10;

    [SerializeField] private float inkGained = 1f;


    [SerializeField] private GameObject deathEffect;

    // Flags for status effects

    private bool isScorched = false;
    private bool isChilled = false;
    private bool isDrenched = false;
    private bool isScalded = false;
    private bool isFrozen = false;
    private bool isWeakened = false;
   
    public bool isInFog = true;
    //private MeshRenderer ballMeshRenderer;
    private Transform ballParentTransform;

    private Transform target;
    private int waypointIndex = 0;

    private int lastXCoord = 0;
    private int lastYCoord = 0;
    
    // to reduce bullet damage
    private GameObject bulletPrefab;

    [Header("Unity Stuff")]
    public Image healthBar;
    
    public MapGenerator map;
    private Cell[,] cells;

    public GameObject model;
    public Animator animator;

    public void TakeDamage(float amount)
    {
        health = health - amount + defense;

        // float number between 0 and 1
        healthBar.fillAmount = health / initHealth;

        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeDot(float amount)
    {
        health = health - amount;

        // float number between 0 and 1
        healthBar.fillAmount = health / initHealth;

        if (health <= 0)
        {
            Die();
        }
    }

    public void ReduceSpeed(float slowAmount)
    {
        speed = initSpeed * slowAmount;
    }

    public void RestoreSpeed()
    {
        speed = initSpeed;
    }

    public void ReduceAttack(int atkDecreAmount)
    {
        bulletPrefab.GetComponent<EnemyBullet>().ReduceBulletDamage(atkDecreAmount);
    }

    public void RestoreAttack()
    {
        bulletPrefab.GetComponent<EnemyBullet>().RestoreBulletDamage();
    }

    public void ReduceDefense(int defDecreAmount)
    {
        defense = initDefense - defDecreAmount;
    }

    public void RestoreDefense()
    {
        defense = initDefense;
    }

    public void setScorched(bool b)
    {
        isScorched = b;
    }

    public bool getScorched()
    {
        return isScorched;
    }

    public void setChilled(bool b)
    {
        isChilled = b;
    }

    public bool getChilled()
    {
        return isChilled;
    }

    public void setDrenched(bool b)
    {
        isDrenched = b;
    }

    public bool getDrenched()
    {
        return isDrenched;
    }

    public void setScalded(bool b)
    {
        isScalded = b;
    }

    public bool getScalded()
    {
        return isScalded;
    }

    public void setFrozen(bool b)
    {
        isFrozen = b;
    }

    public bool getFrozen()
    {
        return isFrozen;
    }

    public void setWeakened(bool b)
    {
        isWeakened = b;
    }

    public bool getWeakened()
    {
        return isWeakened;
    }

    public bool getInFog()
    {
        return isInFog;
    }

    void Die ()
    {
        // add ink
        Player playerScript = GameObject.Find("Player").GetComponent<Player>();
        playerScript.AddInk(inkGained);

        // for new wave
        WaveSpawner.numEnemiesAlive--;
        
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        Destroy(gameObject);
  
    }

    void Start()
    {
        // Initialize Stats
        health = initHealth;
        speed = initSpeed;
        defense = initDefense;
        bulletPrefab = GetComponentInParent<EnemyShooting>().bulletPrefab;

        // initialize model and animator
        model = transform.GetChild(2).gameObject;
        animator = model.GetComponent<Animator>();

        //ballMeshRenderer = gameObject.GetComponent<MeshRenderer>();
        ballParentTransform = gameObject.transform;

        // first target, which is first waypoint in Waypoints
        target = Waypoints.points[0];
        
        // get a reference to all cells for checking if a tile is fogged or not
        cells = GameObject.Find("Map").GetComponent<MapGenerator>().GetCells();
    }


    void Update()
    {
        if (GetComponent<EnemyShooting>().isShooting) {
            // stop movement
            animator.SetBool("isWalking", false);
            return;
        }

        animator.SetBool("isWalking", true);

        // movement direction to the target waypoint
        Vector3 direction = target.position - transform.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(model.transform.rotation, lookAtRotation, Time.deltaTime * 10f).eulerAngles;
        model.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

        // delta time is time passed since last frame
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        
        var currentPosition = transform.position;

        if (Vector3.Distance(currentPosition, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }

        int currentXCoord = Convert.ToInt32(Math.Floor(currentPosition.x));
        int currentYCoord = Convert.ToInt32(Math.Floor(currentPosition.z));

        if (currentXCoord != lastXCoord || currentYCoord != lastYCoord)
        {
            lastXCoord = currentXCoord;
            lastYCoord = currentYCoord;
/*            Debug.Log((lastXCoord, lastYCoord));*/
            isInFog = GetCurrentTileFogged(currentXCoord, currentYCoord);
        }
        
        // enemy is visible if not in fog, hence its visibility is the negation of the isInFog bool.
        setEnemyVisibility(!isInFog);
        
    }

    private bool GetCurrentTileFogged(int xCoord, int yCoord)
    {
        Cell cell = cells[yCoord, xCoord];
        return cell.isFog;
    }

    void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.points.Length - 1) 
        {
            EndPath();
            return;
        }
        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }

    // previously was enemy attack the base and destroy
    void EndPath()
    {
        // do nothing
        animator.SetBool("isWalking", false);
    }

    private void setEnemyVisibility(bool isVisible)
    {
        //ballMeshRenderer.enabled = isVisible;
        foreach (Transform childrenTransform in ballParentTransform)
        {
            childrenTransform.gameObject.SetActive(isVisible);
        }
    }

}

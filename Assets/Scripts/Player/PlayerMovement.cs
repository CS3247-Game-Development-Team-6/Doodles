using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour {
    /*
    This is the main script for the player that handles most inputs for actions

    Requires player game object to have a "ActionTimer" game object containing a TMP_Text.
    Requires player game object to have a "FirePoint" game object containing a Transform.
    */
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask tileAndFogLayerMask;
    private TowerManager towerManager;
    private InkManager inkManager;
    private IndicatorUI buildIndicator;
    public GameObject playerGO;
    

    // states
    private enum State { // used for player actions that cannot be interrupted
        Normal,
        Rolling,
        Attacking,
        Paused,
    }
    private State state;

    // player movement values
    private float playerSpeed;
    // private float sprintSpeed = 8f; // TODO: add sprinting if needed in the future
    private float walkSpeed = 4f;
    private float initialRollSpeed = 50f;
    private float currentRollSpeed;

    // action boolean checks
    // private bool isSprinting = false; // TODO: add sprinting if needed in the future
    private bool isBuilding = false;
    private bool isUsingShooting;
    private bool isAttacking = false;
    private bool isPaused = false;

    // player build values
    [SerializeField] private float buildDistance = 1.5f;
    [SerializeField] private float buildDuration = 4f;
    private float currentBuildDuration = 0f;
    private GameObject currentTowerCell; // current cell that the player is interacting with

    // player attack values
    private float attackDuration = 0.25f;
    private float currentAttackDuration = 0f;

    // player unity object attributes
    private Animator animator;
    private Rigidbody rigidBody;
    public Camera playerCamera;
    private TMP_Text actionTimer;
    private PlayerShooting playerShootingScript;
    private PlayerMelee playerMeleeScript;
    private Transform firePoint;
    private PauseMenu pauseMenu;


    // directions and positions
    private Vector3 moveDirection;
    private Vector3 lastMoveDirection;
    private Vector3 rollDirection;
    private Vector3 mousePositionVector;

    // Start is called before the first frame update
    private void Start() {
        playerSpeed = walkSpeed;
        // animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();

        inkManager = InkManager.instance;
        towerManager = TowerManager.instance;

        // initialize action timer text
        actionTimer = GameObject.Find("ActionTimer").GetComponent<TMP_Text>();
        actionTimer.text = "";

        // initialize melee and shooting; shooting is default
        playerShootingScript = GetComponent<PlayerShooting>();
        playerMeleeScript = GetComponent<PlayerMelee>();
        playerShootingScript.disableShooting();
        playerMeleeScript.enableMelee();
        isUsingShooting = false;
        firePoint = GameObject.Find("FirePoint").GetComponent<Transform>();

        // initialize reference to pause menu
        pauseMenu = GameObject.Find("PauseCanvas").GetComponent<PauseMenu>();
        
        // set default state
        state = State.Normal;

        buildIndicator = GetComponent<IndicatorUI>();

        if (buildIndicator) {
            // set maxValue of buildIndicator slider
            buildIndicator.maxValue = (int)Mathf.Round(buildDuration * 1000);
            buildIndicator.rawValue = 0;
        }
    }

    // Update is called once per frame
    private void Update() {
        UpdatePauseState(); // update whether game is paused

        switch (state) {
            default:
                // game is running normally
                ProcessInputs();
                Ray mouseRay = playerCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, groundLayerMask)) {
                    mousePositionVector = raycastHit.point;
                    Debug.DrawLine(mouseRay.origin, raycastHit.point);
                    // mousePositionVector.y = transform.position.y; // set to same vertical height as player
                }
                break;

            case State.Paused:
                // game is paused, pause all updates for player
                break;
        }
    }

    // Update is called at a fixed rate
    private void FixedUpdate() {
        switch (state) {
            default:
                Move();
                Build();
                Attack();

                Vector3 lookDirection = (mousePositionVector - rigidBody.position).normalized;
                float angle = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
                firePoint.eulerAngles = new Vector3(0, angle, 0);
                // rigidBody.rotation = new Vector3(0, angle, 0); // TODO: player should not rotate; should change sprite instead
                // TODO: add player sprite animation
                break;
            case State.Paused:
            // game is paused, pause all updates for player
            break;
        }
        
    }

    public bool GetIsBuilding() {
        return isBuilding;
    }

    public float GetBuildDistance() {
        return buildDistance;
    }

    private void UpdatePauseState() {
        if (pauseMenu.IsPaused()) {
            // game is paused, update state
            isPaused = true;
            state = State.Paused;
            playerShootingScript.Pause();
            playerMeleeScript.Pause();
        } else {
            // game is not paused
            if (!isPaused) { // game is already unpaused, do not update state
                return;
            }

            // update state
            isPaused = false;
            state = State.Normal;
            playerShootingScript.Resume();
            playerMeleeScript.Resume();
        }
    }
    private void ProcessInputs() {
        switch (state) {
            case State.Normal:
                HandleMovementInputs();
                HandleBuildInputs();
                HandleWeaponSwapInputs();
                HandleAttackInputs();
                break;

            case State.Rolling:
                // TODO: add player rolling animation
                float speedDropMultiplier = 20f;
                currentRollSpeed -= currentRollSpeed * speedDropMultiplier * Time.deltaTime;
            
                float rollSpeedMinimum = 20f;
                if (currentRollSpeed < rollSpeedMinimum) {
                    state = State.Normal;
                }
                break;

            case State.Attacking: // used to wait for attack animations
                break;
            }
    }

    private void HandleMovementInputs() {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = 0;
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(moveX, moveY, moveZ).normalized;
        if (moveDirection.x != 0 || moveDirection.z != 0) {
            // player is moving
            lastMoveDirection = moveDirection; // used for movement actions when not moving
        }

        // TODO: fix rolling (it causes clipping)
        // if (Input.GetKeyDown(KeyCode.Space)) {
        //     rollDirection = lastMoveDirection;
        //     currentRollSpeed = initialRollSpeed;
        //     state = State.Rolling;
        // }
    }

    private void HandleBuildInputs() {
        if (Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.A) || 
            Input.GetKeyDown(KeyCode.S) || 
            Input.GetKeyDown(KeyCode.D) || 
            // Input.GetMouseButtonDown(0) || 
            // Input.GetMouseButtonDown(1) ||
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.F)) {
            // interrupt building action on other action inputs
            isBuilding = false; 
            actionTimer.text = "";
            buildIndicator.rawValue = 0;
        }
    }

    private void HandleWeaponSwapInputs() {
        if (Input.GetKeyDown(KeyCode.Q)) { // use Q to swap weapon
            if (isUsingShooting) {
                // currently using shooting, swap to melee
                playerShootingScript.disableShooting();
                playerMeleeScript.enableMelee();
                isUsingShooting = false;
            } else {
                // currently using melee, swap to shooting
                playerShootingScript.enableShooting();
                playerMeleeScript.disableMelee();
                isUsingShooting = true;
            }
        }
    }

    private void HandleAttackInputs() {
        if (Input.GetMouseButtonDown(0)) { // left click (tied to attacking action)
            moveDirection = Vector3.zero; // prevents character sliding while attacking
            // TODO: add animation for attack action
            currentAttackDuration = attackDuration;
            state = State.Attacking;
            isAttacking = true;
            // animator.PlayAttackAnimation(attackDirection, () => state = State.Normal);
        }
    }

    private void Move() {
        switch(state) {
        case State.Normal:
            rigidBody.velocity = moveDirection * playerSpeed;

            if (rigidBody.velocity.magnitude > 0.2) {
                // interrupt building action
                isBuilding = false; 
                actionTimer.text = "";
                buildIndicator.rawValue = 0;
            }

            break;

        case State.Rolling:
            rigidBody.velocity = rollDirection * currentRollSpeed;
            break;

        case State.Attacking:
            rigidBody.velocity = Vector3.zero;
            break;
        }
    }

    /////////////////////////////////////
    //
    // Building-related functions
    //
    /////////////////////////////////////
    public void BuildTowerAttempt(Vector3 mouseTowerCellPosition, GameObject towerCell) {

        currentTowerCell = towerCell;
        
        // Cannot interact with out of range towers.
        if ((mouseTowerCellPosition - transform.position).magnitude > buildDistance) { 
            return;
        }

        Node node = currentTowerCell.GetComponent<Node>();
        // HasTower() opens NodeUI if there is a tower.
        if (node.HasTower()) {
            // tower cell already has a tower
            towerManager.SelectNode(node);
            return;
        }

        if (isBuilding) { 
            return;
        }

        // Ink cost
        if (!inkManager.hasEnoughInk(towerManager.GetTowerCost())) {
            towerManager.TriggerInsufficientInk();
            return;
        }

        // Are we allowed to build the tower here?
        if (!towerManager.CanBuildTower(node)) {
            // node.TriggerInvalidAction();
            Debug.LogWarning($"Cannot build tower on {node}");
            return;
        }

        currentBuildDuration = buildDuration;
        isBuilding = true;
        Build();
    }

    private void Build() {
        if (!isBuilding) {
            return;
        }

        // TODO: add player building animation
        currentBuildDuration -= Time.deltaTime;
        if (currentBuildDuration > 0) {
            actionTimer.text = (Mathf.Round(currentBuildDuration * 10) / 10).ToString(); // display timer
            buildIndicator.rawValue = (int) Mathf.Round(currentBuildDuration * 1000);
        }

        if (currentBuildDuration <= 0) {
            actionTimer.text = ""; // stop displaying timer
            buildIndicator.rawValue = 0;
            towerManager.BuildTower(currentTowerCell.GetComponent<Node>());
            isBuilding = false;
        }
    }

    private void Attack() { // used to prevent player input for a short duration when attacking
        if (!isAttacking) { // player is not attacking
            return;
        }

        currentAttackDuration -= Time.deltaTime;
        if (currentAttackDuration <= 0) {
            state = State.Normal;
            isAttacking = false;
        }
    }
}
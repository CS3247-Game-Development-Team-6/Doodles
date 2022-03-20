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
    [SerializeField] private LayerMask dashLayerMask;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask tileAndFogLayerMask;
    [SerializeField] private Player player;

    // states
    private enum State { // used for player actions that cannot be interrupted
        Normal,
        Rolling,
        Attacking,
    }
    private State state;

    // player movement values
    private float playerSpeed;
    private float sprintSpeed = 8f; // TODO: add sprinting if needed in the future
    private float walkSpeed = 4f;
    private float dashAmount = 3f;
    private float initialRollSpeed = 50f;
    private float currentRollSpeed;

    // action boolean checks
    private bool isSprinting = false; // TODO: add sprinting if needed in the future
    private bool isDashing = false;
    private bool isBuilding = false;
    private bool isUsingShooting;

    // player build values
    private float buildDistance = 3f;
    [SerializeField]
    private float buildDuration = 5f;
    private float currentBuildDuration = 0f;
    private GameObject currentTowerCell; // current cell that the player is interacting with

    // player unity object attributes
    private Animator animator;
    private Rigidbody rigidBody;
    public Camera playerCamera;
    private TMP_Text actionTimer;
    private PlayerShooting playerShootingScript;
    private PlayerMelee playerMeleeScript;
    private Transform firePoint;


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

        // initialize action timer text
        actionTimer = GameObject.Find("ActionTimer").GetComponent<TMP_Text>();
        actionTimer.text = "";

        // initialize melee and shooting; shooting is default
        playerShootingScript = GetComponent<PlayerShooting>();
        playerMeleeScript = GetComponent<PlayerMelee>();
        playerShootingScript.enableShooting();
        playerMeleeScript.disableMelee();
        isUsingShooting = true;
        firePoint = GameObject.Find("FirePoint").GetComponent<Transform>();
        
        // set default state
        state = State.Normal;
    }

    // Update is called once per frame
    private void Update() {
        ProcessInputs();
        Ray mouseRay = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, groundLayerMask)) {
            mousePositionVector = raycastHit.point;
            // mousePositionVector.y = transform.position.y; // set to same vertical height as player
        }
        
        // Debug.Log(Physics.Raycast(mouseRay, out RaycastHit test, float.MaxValue, groundLayerMask));
        // Debug.Log(mousePositionVector);
    }

    // Update is called at a fixed rate
    private void FixedUpdate() {
        Move();
        Build();

        Vector3 lookDirection = (mousePositionVector - rigidBody.position).normalized;
        float angle = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
        firePoint.eulerAngles = new Vector3(0, angle, 0);
        // rigidBody.rotation = new Vector3(0, angle, 0); // TODO: player should not rotate; should change sprite instead
        // TODO: add player sprite animation
    }

    private void ProcessInputs() {
        switch (state) {
            case State.Normal:
                HandleMovementInputs();
                HandleBuildInputs();
                HandleWeaponSwapInputs();

                if (Input.GetMouseButtonDown(0)) { // left click (tied to attacking action)
                    moveDirection = Vector3.zero; // prevents character sliding while attacking
                    // TODO: add animation for attack action
                    // state = State.Attacking;
                    // animator.PlayAttackAnimation(attackDirection, () => state = State.Normal);
                }
                break;

            case State.Rolling:
                // TODO: add player rolling animation
                float speedDropMultiplier = 10f;
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

        if (Input.GetKeyDown(KeyCode.F)) { // TODO: dashing tentatively tied to key F for testing
            isDashing = true;
        }
    
        if (Input.GetKeyDown(KeyCode.Space)) {
            rollDirection = lastMoveDirection;
            currentRollSpeed = initialRollSpeed;
            state = State.Rolling;
        }
    }

    private void HandleBuildInputs() {
        if (Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.A) || 
            Input.GetKeyDown(KeyCode.S) || 
            Input.GetKeyDown(KeyCode.D) || 
            Input.GetMouseButtonDown(0) || 
            Input.GetMouseButtonDown(1) ||
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.F)) {
            // interrupt building action on other action inputs
            isBuilding = false; 
            actionTimer.text = "";
        }

        if (Input.GetMouseButtonDown(1)) { // right click
            Debug.Log("Attempting to build!"); // TODO: remove
            Ray mouseRay = playerCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, tileAndFogLayerMask)) {
                // Replace with actual tile layer, remove hard coding.
                if (raycastHit.collider.gameObject.layer == 11) { // right clicked on a TowerCell
                    Debug.Log("Clicked on " + raycastHit.collider.gameObject.name); // TODO: remove

                    GameObject towerCell = raycastHit.collider.gameObject;
                    Vector3 mouseTowerCellPosition = raycastHit.point;
                    BuildTowerAttempt(mouseTowerCellPosition, towerCell);
                }
            }
        }
    }

    private void HandleWeaponSwapInputs() {
        if (Input.GetKeyDown(KeyCode.Q)) { // use Q to swap weapon
            Debug.Log("Swapped weapon!"); // TODO: remove
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

    private void Move() {
        switch(state) {
        case State.Normal:
            rigidBody.velocity = moveDirection * playerSpeed;

            // TODO: add player moving animation

            if (rigidBody.velocity.magnitude > 0.2) {
                // interrupt building action
                isBuilding = false; 
                actionTimer.text = "";
            }

            // Dashing would be a replacement or an upgrade to rolling for now
            if (isDashing) {
                Vector3 dashPosition = transform.position + lastMoveDirection * dashAmount;

                RaycastHit2D raycastHit2d = Physics2D.Raycast(transform.position, moveDirection, dashAmount, dashLayerMask);
                if (raycastHit2d.collider != null) {
                    // hit something
                    dashPosition = raycastHit2d.point;
                }
                rigidBody.MovePosition(dashPosition);
                // TODO: add player dashing animation
                isDashing = false;
            }
            break;

        case State.Rolling:
            rigidBody.velocity = rollDirection * currentRollSpeed;
            break;
        }
    }

    private void BuildTowerAttempt(Vector3 mouseTowerCellPosition, GameObject towerCell) {
        
        if ((mouseTowerCellPosition - transform.position).magnitude > buildDistance) { 
            // player too far from tower cell
            return;
        }

        if (isBuilding) { 
            // player already building a tower
            return;
        }

        currentTowerCell = towerCell;
        if (!player.hasEnoughInk(currentTowerCell.GetComponent<Node>().TowerCost())) {
            // player has not enough ink
            Debug.Log("Not enough ink!");
            return;
        }

        if (currentTowerCell.GetComponent<Node>().HasTower()) {
            // tower cell already has a tower
            return;
        }

        currentBuildDuration = buildDuration;
        isBuilding = true;
        Build();
    }

    private void Build() {
        if (!isBuilding) { // player not building anything
            return;
        }

        // TODO: add player building animation
        currentBuildDuration -= Time.deltaTime;
        if (currentBuildDuration > 0) {
            actionTimer.text = (Mathf.Round(currentBuildDuration * 10) / 10).ToString(); // display timer
        }

        if (currentBuildDuration <= 0) {
            actionTimer.text = ""; // stop displaying timer

            Turret turret = currentTowerCell.GetComponent<Node>().BuildTower();
            if (turret != null) {
                player.ChangeInkAmount(-turret.Cost);
            }
            isBuilding = false;
        }
    }
}
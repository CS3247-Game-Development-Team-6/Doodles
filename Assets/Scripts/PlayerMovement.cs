using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    [SerializeField] private LayerMask dashLayerMask;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask tileLayerMask;

    // states
    private enum State { // used for player actions that cannot be interrupted
        Normal,
        Rolling,
    }
    private State state;

    // player movement values
    private float playerSpeed;
    private float sprintSpeed = 8f; // TODO: add sprinting if needed in the future
    private float walkSpeed = 4f;
    private float dashAmount = 3f;
    private float initialRollSpeed = 50f;
    private float currentRollSpeed;

    // player values
    private float buildDistance = 10f;
    private float buildDuration = 3f;
    private float currentBuildDuration = 0f;
    private GameObject currentTowerCell; // current cell that the player is interacting with

    // boolean checks
    private bool isSprinting = false; // TODO: add sprinting if needed in the future
    private bool isDashing = false;
    private bool isBuilding = false;
    
    // player unity object attributes
    private Animator anim;
    private Rigidbody rigidBody;
    public Camera playerCamera;

    // directions and positions
    private Vector3 moveDirection;
    private Vector3 lastMoveDirection;
    private Vector3 rollDirection;
    private Vector3 mousePositionVector;

    // Start is called before the first frame update
    private void Awake() {
        playerSpeed = walkSpeed;
        // anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
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

        Vector3 lookDirection = mousePositionVector - rigidBody.position;
        float angle = Mathf.Atan2(lookDirection.z, lookDirection.x) * Mathf.Rad2Deg - 90f;
        // rigidBody.rotation = angle; // TODO: player should not rotate; should change sprite instead
        // TODO: add player sprite animation
    }

    private void ProcessInputs() {
        switch (state) {
            case State.Normal:
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

                if (Input.GetMouseButtonDown(1)) { // right click
                    Ray mouseRay = playerCamera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, tileLayerMask)) {
                        if (raycastHit.collider.gameObject.layer == 11) { // right clicked on a TowerCell
                            Debug.Log("Clicked on " + raycastHit.collider.gameObject.name);
                            GameObject towerCell = raycastHit.collider.gameObject;
                            Vector3 mouseTowerCellPosition = raycastHit.point;
                            BuildTowerAttempt(mouseTowerCellPosition, towerCell);

                        }
                    }
                }

                if (Input.GetMouseButtonDown(0)) { // left click (tied to attacking action)
                    isBuilding = false; // interrupts building action
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
            }
        
    }

    private void Move() {
        switch(state) {
        case State.Normal:
            rigidBody.velocity = moveDirection * playerSpeed;

            // TODO: add player moving animation

            if (rigidBody.velocity.magnitude > 0.2) {
                isBuilding = false; // interrupt building action
            }

            // Dashing would be a replacement or an upgrade to rolling for now
            if (isDashing) {
                isBuilding = false; // interrupt building action

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
        Debug.Log((mouseTowerCellPosition - transform.position).magnitude);
        Debug.Log(buildDistance);
        if ((mouseTowerCellPosition - transform.position).magnitude > buildDistance) { // player too far from tower cell
            return;
        }

        if (isBuilding) { // player already building a tower
            return;
        }
        currentTowerCell = towerCell;
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
        if (currentBuildDuration <= 0) {
            currentTowerCell.GetComponent<Node>().BuildTower(); // TODO: get buildTower() working
            isBuilding = false;
        }
    }
}

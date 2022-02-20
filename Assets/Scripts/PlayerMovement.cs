using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    [SerializeField] private LayerMask dashLayerMask;
    [SerializeField] private LayerMask groundLayerMask;

    // states
    private enum State {
        Normal,
        Rolling,
    }
    private State state;

    // movement values
    private float playerSpeed;
    public float sprintSpeed = 8f;
    public float walkSpeed = 4f;
    public float dashAmount = 3f;
    public float initialRollSpeed = 50f;
    public float currentRollSpeed;

    // boolean checks
    private bool isMoving = false;
    private bool isSprinting = false;
    private bool isDashing = false;
    
    // player attributes
    private Animator anim;
    private Rigidbody rigidBody;
    public Camera camera;

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
        Ray mouseRay = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, groundLayerMask)) {
            mousePositionVector = raycastHit.point;
            //mousePositionVector.y = transform.position.y; // set to same vertical height as player
        }
        Debug.Log(Physics.Raycast(mouseRay, out RaycastHit test, float.MaxValue, groundLayerMask));
        Debug.Log(mousePositionVector);
    }

    // Update is called at a fixed rate
    private void FixedUpdate() {
        Move();

        Vector3 lookDirection = mousePositionVector - rigidBody.position;
        float angle = Mathf.Atan2(lookDirection.z, lookDirection.x) * Mathf.Rad2Deg - 90f;
        // rigidBody.rotation = angle; // TODO: player should not rotate; should change sprite instead
    }

    private void ProcessInputs() {
        switch (state) {
        case State.Normal:
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = 0;
            float moveZ = Input.GetAxisRaw("Vertical");

            moveDirection = new Vector3(moveX, moveY, moveZ).normalized;
            if (moveDirection.x != 0 || moveDirection.z != 0) {
                // is moving
                lastMoveDirection = moveDirection;
            }

            if (Input.GetKeyDown(KeyCode.F)) {
                isDashing = true;
            }
            
            if (Input.GetKeyDown(KeyCode.Space)) {
                rollDirection = lastMoveDirection;
                currentRollSpeed = initialRollSpeed;
                state = State.Rolling;
            }
            break;

        case State.Rolling:
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

            if (isDashing) {
                Vector3 dashPosition = transform.position + lastMoveDirection * dashAmount;

                RaycastHit2D raycastHit2d = Physics2D.Raycast(transform.position, moveDirection, dashAmount, dashLayerMask);
                if (raycastHit2d.collider != null) {
                    // hit something
                    dashPosition = raycastHit2d.point;
                }
                rigidBody.MovePosition(dashPosition);
                isDashing = false;
            }
            break;

        case State.Rolling:
            rigidBody.velocity = rollDirection * currentRollSpeed;
            break;
        }
    }
}

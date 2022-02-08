using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    public float playerSpeed;
    public float sprintSpeed = 4f;
    public float walkSpeed = 2f;
    private bool isMoving = false;
    private bool isSprinting =false;
    
    public Animator anim;
    public Rigidbody rigidBody;

    private Vector3 moveDirection;

    // Start is called before the first frame update
    void Start() {
        playerSpeed = walkSpeed;
        // anim = GetComponent<Animator>();
        // rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        ProcessInputs();
    }

    // Update is called at a fixed rate
    void FixedUpdate() {
        Move();
    }

    void ProcessInputs() {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = 0;
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(moveX, moveY, moveZ).normalized;
    }

    void Move() {
        rigidBody.velocity = new Vector3(moveDirection.x * playerSpeed, 
                moveDirection.y * playerSpeed, 
                moveDirection.z * playerSpeed);
    }
}

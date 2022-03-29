using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour {
    /*
    Requires player game object to have a "FirePoint" game object containing a Transform.
    */
    [SerializeField] private LayerMask groundLayerMask;

    private Transform firePoint; // TODO: firepoint may need changes to rotation
    public GameObject meleeHitboxPrefab; 

    private float meleeRange = 1f;
    private float meleeCooldown = 0.5f; 
    private float currentCooldown = 0f;

    private Camera mainCamera;

    private Vector3 mousePositionVector;
    private Vector3 meleeDirection;
    private bool isUsingMelee;

    // states
    private enum State { 
        Normal,
        Paused,
    }
    private State state;

    private void Start() {
        firePoint = GameObject.Find("FirePoint").GetComponent<Transform>();
        mainCamera = Camera.main;
        isUsingMelee = false;
        state = State.Normal;
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            default:
                Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, groundLayerMask)) {
                    mousePositionVector = raycastHit.point;
                    //mousePositionVector.y = transform.position.y; // set to same vertical height as player
                }

                if (isUsingMelee && Input.GetButtonDown("Fire1")) {
                    MeleeAttack();
                }
                break;
            case State.Paused:
                break;
        }
    }

    void FixedUpdate() {
        switch (state) {
            default:
                meleeDirection = (mousePositionVector - firePoint.position).normalized;

                if (currentCooldown > 0) {
                    currentCooldown -= Time.deltaTime;
                }
                break;
            case State.Paused:
                break;
        }
    }

    public void Pause() {
        state = State.Paused;
    }

    public void Resume() {
        state = State.Normal;
    }

    public void enableMelee() {
        isUsingMelee = true;
    }

    public void disableMelee() {
        isUsingMelee = false;
    }

    void MeleeAttack() {
        if (currentCooldown <= 0) {
            Vector3 attackPosition = transform.position + meleeDirection * meleeRange; 
            // TODO: refine size of hitbox
            GameObject meleeHitbox = Instantiate(meleeHitboxPrefab, attackPosition, firePoint.rotation);
            
            // TODO: add melee animation somewhere
            currentCooldown = meleeCooldown;
        }
    }
}

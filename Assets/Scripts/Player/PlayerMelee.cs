using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour {
    /*
    Requires player game object to have a "FirePoint" game object containing a Transform.
    */
    [SerializeField] private LayerMask groundLayerMask;

    private Transform firePoint; // TODO: firepoint may need changes to rotation
    public GameObject meleePrefab; // TODO: get an actual melee prefab

    private float bulletForce = 4f;
    private float meleeCooldown = 0.5f; 
    private float currentCooldown = 0f;

    private Camera mainCamera;

    private Vector3 mousePositionVector;
    private Vector3 meleeDirection;
    private bool isUsingMelee;

    private void Start() {
        firePoint = GameObject.Find("FirePoint").GetComponent<Transform>();
        mainCamera = Camera.main;
        isUsingMelee = false;
    }

    // Update is called once per frame
    void Update() {
        Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, groundLayerMask)) {
            mousePositionVector = raycastHit.point;
            //mousePositionVector.y = transform.position.y; // set to same vertical height as player
        }

        if (isUsingMelee && Input.GetButtonDown("Fire1")) {
            MeleeAttack();
        }
    }

    void FixedUpdate() {
        meleeDirection = mousePositionVector - firePoint.position;

        if (currentCooldown > 0) {
            currentCooldown -= Time.deltaTime;
        }
    }

    public void enableMelee() {
        isUsingMelee = true;
    }

    public void disableMelee() {
        isUsingMelee = false;
    }

    void MeleeAttack() {
        if (currentCooldown <= 0) {
            // TODO: add actual melee attack hitbox
            
            // TODO: add melee animation somewhere
            currentCooldown = meleeCooldown;
        }
    }
}

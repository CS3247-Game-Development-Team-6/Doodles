using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
    /*
    Requires player game object to have a "FirePoint" game object containing a Transform.
    */
    [SerializeField] private LayerMask groundLayerMask;

    private Transform firePoint; // TODO: firepoint may need changes to rotation
    public GameObject bulletPrefab; // TODO: get an actual bullet prefab

    private float bulletForce = 4f;
    private float shootingCooldown = 0.2f; 
    private float currentCooldown = 0f;

    private Camera mainCamera;

    private Vector3 mousePositionVector;
    private Vector3 bulletDirection;
    private bool isUsingShooting;

    private void Start() {
        firePoint = GameObject.Find("FirePoint").GetComponent<Transform>();
        mainCamera = Camera.main;
        isUsingShooting = true;
    }

    // Update is called once per frame
    void Update() {
        Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, groundLayerMask)) {
            mousePositionVector = raycastHit.point;
            //mousePositionVector.y = transform.position.y; // set to same vertical height as player
        }

        if (isUsingShooting && Input.GetButtonDown("Fire1")) {
            Shoot();
        }
    }

    void FixedUpdate() {
        bulletDirection = mousePositionVector - firePoint.position;

        if (currentCooldown > 0) {
            currentCooldown -= Time.deltaTime;
        }
    }

    public void enableShooting() {
        isUsingShooting = true;
    }

    public void disableShooting() {
        isUsingShooting = false;
    }

    private void Shoot() {
        if (currentCooldown <= 0) {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rigidBody = bullet.GetComponent<Rigidbody>();
            rigidBody.AddForce(bulletDirection * bulletForce, ForceMode.Impulse);
            // TODO: add shooting animation somewhere
            currentCooldown = shootingCooldown;
        }
    }
}

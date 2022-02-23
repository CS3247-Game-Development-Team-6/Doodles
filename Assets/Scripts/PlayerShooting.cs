using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
    [SerializeField] private LayerMask groundLayerMask;

    public Transform firePoint; // TODO: firepoint may need changes to rotation
    public GameObject bulletPrefab; // TODO: get an actual bullet prefab

    public float bulletForce = 4f;
    public float shootingCooldown = 0.5f; 
    private float currentCooldown = 0f;

    public Camera camera;

    private Vector3 mousePositionVector;
    private Vector3 bulletDirection;

    private void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Ray mouseRay = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, groundLayerMask)) {
            mousePositionVector = raycastHit.point;
            //mousePositionVector.y = transform.position.y; // set to same vertical height as player
        }

        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
    }

    void FixedUpdate() {
        bulletDirection = mousePositionVector - firePoint.position;

        if (currentCooldown > 0) {
            currentCooldown -= Time.deltaTime;
        }
    }

    void Shoot() {
        if (currentCooldown <= 0) {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rigidBody = bullet.GetComponent<Rigidbody>();
            rigidBody.AddForce(bulletDirection * bulletForce, ForceMode.Impulse);
            currentCooldown = shootingCooldown;
        }
    }
}

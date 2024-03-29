using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMelee : MonoBehaviour {
    /*
    Requires player game object to have a "FirePoint" game object containing a Transform.
    */
    [SerializeField] private LayerMask groundLayerMask;

    private Transform firePoint;
    public GameObject meleeHitboxPrefab;
    public GameObject meleeWeaponPrefab;
    public GameObject meleeSwingSoundPrefab;

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
        isUsingMelee = true;
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

                if (!IsPointerOverUIObject() && isUsingMelee && Input.GetButtonDown("Fire1")) {
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
            Vector3 hitboxPosition = transform.position + meleeDirection * meleeRange; 
            Vector3 weaponPosition = transform.position + meleeDirection * (meleeRange/2); 
            Vector3 meleeRotation = new Vector3(90, firePoint.eulerAngles.y, firePoint.eulerAngles.z);

            Instantiate(meleeSwingSoundPrefab, hitboxPosition, firePoint.rotation); // create the sound for firing a bullet
            GameObject meleeHitbox = Instantiate(meleeHitboxPrefab, hitboxPosition, firePoint.rotation);
            GameObject meleeWeapon = Instantiate(meleeWeaponPrefab, weaponPosition, Quaternion.Euler(meleeRotation));
            
            currentCooldown = meleeCooldown;
        }
    }

    private bool IsPointerOverUIObject() {
        // the ray cast appears to require only eventData.position.
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}

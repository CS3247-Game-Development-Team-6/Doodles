using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeSwing : MonoBehaviour
{   
    public float swingDuration = 0.15f; // used as life time of swing
    public float swingAngle = 75f;
    private float targetAngle;
    private float currentLifeTime = 0f;

    // Start is called before the first frame update
    void Start() {
        SwingStart();
        targetAngle = transform.eulerAngles.z - (swingAngle/2);
    }

    // Update is called once per frame
    void Update() {
        UpdateLifeTime();
        Swing();
    }

    private void SwingStart() {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + (swingAngle/2));
    }

    private void Swing() {
        float deltaAngle = (Time.deltaTime / swingDuration) * swingAngle;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - deltaAngle);
    }

    private void UpdateLifeTime() {
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime >= swingDuration) {
            Destroy(gameObject);
        }
    }
}

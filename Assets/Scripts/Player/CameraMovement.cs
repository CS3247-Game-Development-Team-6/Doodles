using UnityEngine;

public class CameraMovement : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] public float smoothSpeed;
    [SerializeField] public Vector3 offset = -Vector3.forward * 3;

    private void Update() {
        transform.position = Vector3.Lerp(target.position, transform.position, smoothSpeed) + offset;
        transform.LookAt(target);
    }
}

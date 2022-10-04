using UnityEngine;

[ExecuteInEditMode]
public class CameraMovement : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] public float smoothSpeed;
    [SerializeField] public Vector3 cameraOffset = -Vector3.forward * 3;
    [SerializeField] public Vector3 targetOffset = Vector3.zero;

    private void Update() {
        var pos = target.position + targetOffset;
        transform.position = Vector3.Lerp(pos, transform.position, smoothSpeed) + cameraOffset;
        transform.LookAt(pos);
    }
}

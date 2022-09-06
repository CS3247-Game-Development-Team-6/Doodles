using UnityEngine;

/// <summary>
/// This is sample script
/// </summary>
public class SampleMobMover : MonoBehaviour
{
    private void Awake()
    {
        _startPosition = transform.position;
    }

    [SerializeField] private float _degree;
    [SerializeField] private float _offset;
    private Vector3 _startPosition;
    private float _ownTime;

    void Update()
    {
        var way = Mathf.Sin((_ownTime + _offset) * 0.5f) * 0.5f + 0.5f;
        if (way < 0.2f)
        {
            way = 0.2f;
        }
        else if (way > 0.8f)
        {
            way = 0.8f;
        }

        var newPos = Vector3.Lerp(_startPosition,
            _startPosition + Quaternion.Euler(0, _degree, 0) * Vector3.right * 15f,
            way);
        RaycastHit hit;
        Physics.Raycast(new Ray(transform.position + Vector3.up, Vector3.down), out hit);
        newPos.y = hit.point.y;
        transform.position = newPos;
        _ownTime += Time.deltaTime;
    }
}
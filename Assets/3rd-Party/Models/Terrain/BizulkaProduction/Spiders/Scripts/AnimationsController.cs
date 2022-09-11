using UnityEngine;

[DisallowMultipleComponent]
public class AnimationsController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private bool _animateWhenRun = true;
    private static readonly int MovingHash = Animator.StringToHash("IsMoving");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int IsDeadHash = Animator.StringToHash("IsDead");

    private Vector3 _lastPosition;
    private Quaternion _lastRotation;

    private void Awake()
    {
        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
    }

    private void Update()
    {
        if (_animateWhenRun)
        {
            var tr = transform;
            var position = tr.position;

            if (_lastPosition.x != position.x||_lastPosition.z != position.z)
            {
                _animator.SetBool(MovingHash, true);
                var dirrection = position - _lastPosition;
                dirrection.y = 0;
                _lastRotation = Quaternion.LookRotation(dirrection);
            }
            else
            {
                _animator.SetBool(MovingHash, false);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation,_lastRotation,10f*Time.deltaTime);
            _lastPosition = position;
        }
    }

    private void OnValidate()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
    }

    public void SetMovingState(bool val)
    {
        _animator.SetBool(MovingHash, val);
    }

    public void SetDead()
    {
        _animator.SetBool(IsDeadHash, true);
    }
    public void ClearDead()
    {
        _animator.SetBool(IsDeadHash, false);
        _animator.Play("Idle");
    }
    public void Attack()
    {
        _animator.SetTrigger(AttackHash);
    }

    public void Hit()
    {
        _animator.SetTrigger(HitHash);
    }

    public bool IsMoving
    {
        get { return _animator.GetBool(MovingHash); }
    }
}
using UnityEngine;

public class ClownBullet : EnemyBullet {

    public int amountInkReduce;

    public override void Seek(Transform _target, float _speed, int damage) {
        base.Seek(_target, _speed, damage);
    }

    public override void Damage(Transform _target) {
        base.Damage(_target);

        if (_target.CompareTag("Base") || _target.CompareTag("Player"))
            InkManager.instance.ChangeInkAmount(-amountInkReduce);
    }
}

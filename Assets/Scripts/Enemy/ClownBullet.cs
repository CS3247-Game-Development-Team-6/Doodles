using UnityEngine;

public class ClownBullet : EnemyBullet {

    public int amountInkReduce;

    public override void Seek(Transform _target, float _speed, int damage, bool _enableCameraShake) {
        base.Seek(_target, _speed, damage, _enableCameraShake);
        Debug.Log("seek from clown bullet");
    }

    public override void Damage(Transform _target) {
        base.Damage(_target);
        Debug.Log("damage from clown bullet");
        InkManager.instance.ChangeInkAmount(-amountInkReduce);
    }
}

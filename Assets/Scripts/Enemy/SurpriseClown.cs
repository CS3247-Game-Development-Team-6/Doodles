using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurpriseClown : Enemy {
    private bool dieOnBase;

    public override void Start() {
        base.Start();
        dieOnBase = enemyInfo.dieOnBase;
    }

    public override void EndPath() {
        if (dieOnBase)
            base.Die();

        base.EndPath();
    }
}

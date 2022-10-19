using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To be used for all walls the player builds that can be destroyed by enemies
public interface PlayerWall {
    public void TakeDamage(int damage);
}

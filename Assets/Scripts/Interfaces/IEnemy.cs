using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    // to access an enemy's height so as to determine whether or not it is hittable.
    public int GetCurrentZ();
    // TODO: damage param
    public void Hit(Vector2 hitDirection);
}

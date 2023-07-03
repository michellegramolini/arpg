using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    // TODO: damage param
    public void Hit(Vector2 hitDirection);
}

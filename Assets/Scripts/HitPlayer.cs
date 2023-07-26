using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IAttackable>(out IAttackable attackable))
        {
            attackable.Hit();
        }
    }
}

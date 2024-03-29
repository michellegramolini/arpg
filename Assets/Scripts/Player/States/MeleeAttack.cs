using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : State
{
    private PlayerController _player;
    private bool _melee;

    public override void FixedUpdateState(PlayerController player)
    {
        StopMovement();
    }

    public override void LateUpdateState(PlayerController player)
    {
        Animate();
    }

    public override void StartState(PlayerController player)
    {
        this._player = player;

        StartCoroutine(MeleeSwitch());
        _player.SetLockedMoveVectors();
    }

    public override void UpdateState(PlayerController player)
    {
        if (!_melee)
        {
            _player.isAttacking = false;
            _player.SetState(_player.Idle);
        }

        if (_player.animationState.isDamageFrame)
        {
            HitEnemies();
        }

    }

    private void StopMovement()
    {
        _player.rb.velocity = Vector2.zero;
    }

    private void Animate()
    {
        // use facing direction and set facing animation
        if (_player.locked_facingDirection == Vector2.up)
        {
            _player.animationState.SetAnimationState("player_melee_up");
        }
        else if (_player.locked_facingDirection == Vector2.down)
        {
            _player.animationState.SetAnimationState("player_melee_down");
        }
        // TODO: could handle this at the facingDirection vector level
        else if (_player.locked_facingDirection.x > 0f)
        {
            _player.animationState.SetAnimationState("player_melee_right");
        }
        else
        {
            _player.animationState.SetAnimationState("player_melee_left");
        }

    }

    private IEnumerator MeleeSwitch()
    {
        _melee = true;
        // TODO: replace with attack time
        // TODO: add an animator flag for playing clips and configure on every anim?
        yield return new WaitForSeconds(_player.animationState.GetClipLength("player_melee_right") - 0.1f);
        _melee = false;
    }

    // Activate the hit-box
    private void HitEnemies()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_player.attackPoint, _player.attackRange, _player.enemyLayer);

        if (hitEnemies != null)
        {
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.GetComponent<IEnemy>() != null)
                {
                    // Hit if enemy on same z plane as player
                    if (enemy.GetComponent<IEnemy>().GetCurrentZ() == _player.z)
                    {
                        enemy.GetComponent<IEnemy>().Hit(_player.facingDirection);
                    }

                }
                else if (enemy.GetComponentInParent<IEnemy>() != null)
                {
                    // Hit if enemy on same z plane as player
                    if (enemy.GetComponentInParent<IEnemy>().GetCurrentZ() == _player.z)
                    {
                        enemy.GetComponentInParent<IEnemy>().Hit(_player.facingDirection);
                    }
                }
                else
                {
                    ;
                }
            }
        }
    }
}

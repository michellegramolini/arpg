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
            DoDamage();
        }
    }

    private void StopMovement()
    {
        _player.rb.velocity = Vector2.zero;
    }

    private void Animate()
    {
        // use facing direction and set facing animation
        if (_player.facingDirection == Vector2.up)
        {
            _player.animationState.SetAnimationState("player_melee_up");
        }
        else if (_player.facingDirection == Vector2.down)
        {
            _player.animationState.SetAnimationState("player_melee_down");
        }
        // TODO: could handle this at the facingDirection vector level
        else if (_player.facingDirection.x > 0f)
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
        yield return new WaitForSeconds(0.2f);
        _melee = false;
    }

    // Activate the hit-box
    private void DoDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_player.attackPoint, _player.attackRange, _player.enemyLayer);

        if (hitEnemies != null)
        {
            foreach (Collider2D enemy in hitEnemies)
            {
                //Debug.Log("Hit Enemy and do Damage!");
                enemy.GetComponent<IEnemy>().Hit();
            }
        }
    }
}

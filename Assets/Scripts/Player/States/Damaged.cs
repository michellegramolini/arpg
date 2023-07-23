using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damaged : State
{
    private PlayerController _player;
    private Vector2 _damageDirection;

    public override void FixedUpdateState(PlayerController player)
    {

    }

    public override void StartState(PlayerController player)
    {
        this._player = player;

        StopCoroutine(nameof(DamagedCoroutine));
        StartCoroutine(nameof(DamagedCoroutine));
    }

    public override void UpdateState(PlayerController player)
    {
        // cancel player input?
    }

    public override void LateUpdateState(PlayerController player)
    {
        Animate();
    }

    private void Animate()
    {
        if (_player.facingDirection.x > 0f)
        {
            _player.animationState.SetAnimationState("player_hit_right");
        }
        else if (_player.facingDirection.x < 0f)
        {
            _player.animationState.SetAnimationState("player_hit_left");
        }
        else if (_player.facingDirection.x == 0f && _player.facingDirection.y > 0f)
        {
            _player.animationState.SetAnimationState("player_hit_up");
        }
        else
        {
            _player.animationState.SetAnimationState("player_hit_down");
        }
    }

    private IEnumerator DamagedCoroutine()
    {
        _player.isKnockedBack = true;

        DoDamage();
        DoPopups();

        yield return new WaitForSeconds(0.2f);

        _player.isKnockedBack = false;

        ResetPhysics();
        _player.SetState(_player.Idle);

        yield break;

    }

    private void ResetPhysics()
    {
        _player.rb.velocity = Vector2.zero;
    }

    private void DoDamage()
    {
        // TODO: configure amounts
        _player.health -= 1;
        DamageManager.Instance.AddDamage(1);

    }

    private void DoPopups()
    {
        DamageManager.Instance.GenerateDamagePopup("player_damage_popup", transform.position, 1);
    }
}
